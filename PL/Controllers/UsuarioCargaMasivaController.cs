using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PL.Controllers
{
    public class UsuarioCargaMasivaController : Controller
    {
        private IHostingEnvironment environment;
        private IConfiguration configuration;
        public UsuarioCargaMasivaController(IHostingEnvironment _enviroment, IConfiguration _configuration)
        {
            environment = _enviroment;
            configuration = _configuration;
        }

        public IActionResult GetCargaMasiva()
        {
            ML.Result result = new ML.Result();
            return View(result);
        }

        [HttpPost]
        public IActionResult PostCargaMasiva(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return RedirectToAction("GetCargaMasiva");
            }
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                List<object> registrosExitosos = new List<object>();
                List<string> registrosConError = new List<string>();
                string linea = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    linea = reader.ReadLine();
                    var valores = linea.Split('|');

                    ML.Usuario usuario = new ML.Usuario();
                    usuario.UserName = valores[0];
                    usuario.Nombre = valores[1];
                    usuario.ApellidoPaterno = valores[2];
                    usuario.ApellidoMaterno = valores[3];
                    usuario.Email = valores[4];
                    usuario.Password = valores[5];
                    usuario.Sexo = valores[6];
                    usuario.Telefono = valores[7];
                    usuario.Celular = valores[8];
                    usuario.FechaNacimiento = valores[9];
                    usuario.CURP = valores[10];
                    usuario.Rol = new ML.Rol();
                    usuario.Rol.IdRol = byte.Parse(valores[11]);
                    usuario.Direccion = new ML.Direccion();
                    usuario.Direccion.Calle = valores[12];
                    usuario.Direccion.NumeroInterior = valores[13];
                    usuario.Direccion.NumeroExterior = valores[14];
                    usuario.Direccion.Colonia = new ML.Colonia();
                    usuario.Direccion.Colonia.IdColonia = int.Parse(valores[15]);

                    ML.Result result = BL.Usuario.Add(usuario);
                    if (result.Correct)
                    {
                        registrosExitosos.Add(valores);
                    }
                    else
                    {
                        registrosConError.Add(linea);
                    }
                }
                ViewBag.Message = CrearTxt(registrosConError);
            }
            return RedirectToAction("GetCargaMasiva");
        }

        public static string CrearTxt(List<string> errores)
        {
            string path = "C:/Users/digis/Documents/Gutierrez Ramirez Ricardo/RGutierrezProgramacionNCapasNETCore/PL/wwwroot/archivos/Errores.txt";
            try
            {
                byte[] xd;
                //Comprobamos que el archivo no exista
                if (System.IO.File.Exists(path))
                {
                    //Si el archivo existe, lo elimina
                    System.IO.File.Delete(path);
                }
                //Creamos un nuevo archivo
                using (FileStream archivoTxt = System.IO.File.Create(path))
                {
                    //Escribimos en el archivo creado
                    foreach (var linea in errores)
                    {
                        byte[] texto = new UTF8Encoding(true).GetBytes(linea + "\n");
                        archivoTxt.Write(texto, 0, texto.Length);
                    }
                    return "Archivo creado en: \n" + path;
                }
            }
            catch (Exception ex)
            {
                return "El archivo no se creo debido a: \n" + ex;
            }
        }

        [HttpPost]
        public ActionResult GetCargaMasiva(int? idRegistro)
        {
            IFormFile file = Request.Form.Files["fileExcel"];
            //Validamos si ya se ha creado una sesion
            if (HttpContext.Session.GetString("PathArchivo") == null)
            {
                if (file != null)
                {
                    string nombreArchivo = Path.GetFileName(file.FileName);
                    string extensionArchivo = Path.GetExtension(file.FileName).ToLower();
                    string extencionValida = configuration["TipoExcel"];
                    string carpetaDestino = configuration["PathFolder:ruta"];
                    if (extensionArchivo == extencionValida)
                    {
                        //Código para crear una copia de un archivo
                        string rutaArchivo = Path.Combine(environment.ContentRootPath, carpetaDestino, Path.GetFileNameWithoutExtension(nombreArchivo)) + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                        if (!System.IO.File.Exists(rutaArchivo))
                        {
                            using (FileStream stream = new FileStream(rutaArchivo, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            string connString = configuration["ExcelConString:value"] + rutaArchivo;
                            ML.Result resultExcelData = BL.Usuario.ConvertExcelToDataTable(connString);
                            if (resultExcelData.Correct)
                            {
                                ML.Result resultValidacion = BL.Usuario.ValidarExcel(resultExcelData.Objects);
                                if (resultValidacion.Objects.Count == 0)
                                {
                                    resultValidacion.Correct = true;
                                    //Creamos la sesion que guarde la ruta del archivo
                                    HttpContext.Session.SetString("PathArchivo", rutaArchivo);
                                    ViewBag.Message = "Archivo listo...";
                                }

                                return View(resultValidacion);
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "El archivo no es de tipo Excel";
                    }
                    return View("Modal");
                }
                else
                {
                    ViewBag.Message = "No se inserto el archivo";
                }
                return View("Modal");

            }
            else
            {
                string rutaExcel = HttpContext.Session.GetString("PathArchivo");
                string connectionString = configuration["ExcelConString:value"] + rutaExcel;

                ML.Result resultData = BL.Usuario.ConvertExcelToDataTable(connectionString);
                if (resultData.Correct)
                {
                    ML.Result resultErrores = new ML.Result();
                    resultErrores.Objects = new List<object>();

                    foreach (ML.Usuario objUsuario in resultData.Objects)
                    {
                        ML.Result resultAdd = BL.Usuario.Add(objUsuario);
                        if (!resultAdd.Correct)
                        {
                            resultErrores.Objects.Add("No se inserto el usuario con el nombre: " + objUsuario.Nombre + " Error: " + resultAdd.Message);
                        }
                    }
                    if (resultErrores.Objects.Count > 0)
                    {
                        string errorDeArchivo = Path.Combine(environment.WebRootPath + "\\archivos\\logErrores.txt");
                        using (StreamWriter escribir = new StreamWriter(errorDeArchivo))
                        {
                            foreach (string ln in resultErrores.Objects)
                            {
                                escribir.WriteLine(ln);
                            }
                        }
                        ViewBag.Message = "¡Los usuarios no se registraron!";
                        return View("Modal");
                    }
                    else
                    {
                        ViewBag.Message = "¡Los usuarios se registraron correctamente!";
                        return View("Modal");
                    }
                }
            }
            return RedirectToAction("GetCargaMasiva");
        }
    }
}
