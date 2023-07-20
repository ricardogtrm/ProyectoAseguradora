using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Configuration;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        private IHostingEnvironment environment;
        private IConfiguration configuration;
        public UsuarioController(IHostingEnvironment _enviroment, IConfiguration _configuration)
        {
            environment = _enviroment;
            configuration = _configuration;
        }

        //[HttpGet]
        //public ActionResult GetAll()
        //{
        //    ML.Usuario usuario = new ML.Usuario();
        //    usuario.Nombre = usuario.Nombre == null ? "" : usuario.Nombre;
        //    usuario.ApellidoPaterno = usuario.ApellidoPaterno == null ? "" : usuario.ApellidoPaterno;

        //    ML.Result result = BL.Usuario.GetAll(usuario);
        //    if (result.Correct)
        //    {
        //        usuario.Usuarios = new List<object>();
        //        usuario.Usuarios = result.Objects;
        //        return View(usuario);
        //    }
        //    else
        //    {
        //        ViewBag.Message = result.Message;
        //        return View();
        //    }
        //}

        [HttpPost]
        public ActionResult GetAll(ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.GetAll(usuario);
            if (result.Correct)
            {

                usuario.Usuarios = new List<object>();
                usuario.Usuarios = result.Objects;
                return View(usuario);
            }
            else
            {
                ViewBag.Message = result.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult Form(int? idUsuario)
        {
            ML.Result resultRol = BL.Rol.GetAll();
            ML.Result resultPais = BL.Pais.GetAll();

            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            usuario.Direccion = new ML.Direccion();
            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

            usuario.Rol.Roles = resultRol.Objects;
            usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;

            if (idUsuario == null)
            {
                ViewBag.Titulo = "Agregar nuevo usuario";
                ViewBag.Accion = "Agregar";

                return View(usuario);
            }
            else
            {
                ViewBag.Titulo = "Actualizar un usuario";
                ViewBag.Accion = "Actualizar";
                //ML.Result result = BL.Usuario.GetById(idUsuario.Value);
                ML.Result results = new ML.Result();
                using (HttpClient client = new HttpClient())
                {
                    string webApi = configuration["WebApi"];
                    client.BaseAddress = new Uri(webApi);
                    var responseTask = client.GetAsync("getbyid/" + idUsuario);
                    responseTask.Wait();

                    var resultApi = responseTask.Result;
                    if (resultApi.IsSuccessStatusCode)
                    {
                        var readTask = resultApi.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        ML.Usuario resulUsuario = new ML.Usuario();
                        resulUsuario = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(readTask.Result.Object.ToString());
                        results.Object = resulUsuario;
                        if (results.Object != null)
                        {
                            usuario = ((ML.Usuario)results.Object);
                            ML.Result resultEstado = BL.Estado.GetByIdPais(usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais);
                            ML.Result resultMunicipio = BL.Municipio.GetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                            ML.Result resultColonia = BL.Colonia.GetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);

                            usuario.Rol.Roles = resultRol.Objects;
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
                            usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstado.Objects;
                            usuario.Direccion.Colonia.Municipio.Municipios = resultMunicipio.Objects;
                            usuario.Direccion.Colonia.Colonias = resultColonia.Objects;
                            return View(usuario);
                        }
                        else
                        {
                            ViewBag.Message = results.Message;
                            return View("Modal");
                        }
                        return View("Modal");
                    }
                    else
                    {
                        results.Correct = false;
                        ViewBag.Message = "No existen registros con la tabla usuario";
                        return View("Modal");
                    }
                }


            }
        }

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                ML.Result results = new ML.Result();
                IFormFile image = Request.Form.Files["fileImage"];
                if (image != null)
                {
                    byte[] imageBytes = imageToBytes(image);
                    usuario.Imagen = Convert.ToBase64String(imageBytes);
                }
                if (usuario.IdUsuario == 0)
                {
                    //var result = BL.Usuario.Add(usuario);
                    using (HttpClient client = new HttpClient())
                    {
                        string webApi = configuration["WebApi"];
                        client.BaseAddress = new Uri(webApi);

                        var postTask = client.PostAsJsonAsync<ML.Usuario>("add", usuario);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            ViewBag.Titulo = "¡Registro exitoso!";
                            ViewBag.Message = results.Message;
                            return View("Modal");
                            //return RedirectToAction("GetAll");
                        }
                        else
                        {
                            ViewBag.Titulo = "¡Error al registrar!";
                            ViewBag.Message = Response.StatusCode = 400;
                            return View("Modal");
                        }
                        return View("GetAll");
                    }
                }
                else
                {
                    //var result = BL.Usuario.Update(usuario);
                    using (HttpClient client = new HttpClient())
                    {
                        string webApi = configuration["WebApi"];
                        client.BaseAddress = new Uri(webApi);

                        var postTask = client.PostAsJsonAsync<ML.Usuario>("update", usuario);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            ViewBag.Titulo = "¡Registro exitoso!";
                            ViewBag.Message = results.Message;
                            return View("Modal");
                            //return RedirectToAction("GetAll");
                        }
                        else
                        {
                            ViewBag.Titulo = "¡Error al registrar!";
                            ViewBag.Message = results.Message;
                            return View("Modal");
                        }
                    }
                    return View("GetAll");
                }
            }
            else
            {
                ML.Result resultRol = BL.Rol.GetAll();
                ML.Result resultPais = BL.Pais.GetAll();
                ML.Result resultEstado = BL.Estado.GetByIdPais(usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais);
                ML.Result resultMunicipio = BL.Municipio.GetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                ML.Result resultColonia = BL.Colonia.GetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);

                usuario.Rol.Roles = resultRol.Objects;
                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
                usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstado.Objects;
                usuario.Direccion.Colonia.Municipio.Municipios = resultMunicipio.Objects;
                usuario.Direccion.Colonia.Colonias = resultColonia.Objects;

                return View(usuario);
            }
        }

        //[HttpGet]
        //public ActionResult Delete(int idUsuario)
        //{
        //    var result = BL.Usuario.Delete(idUsuario);

        //    if (result.Correct)
        //    {
        //        ViewBag.Titulo = "¡Registro eliminado!";
        //        ViewBag.Message = result.Message;
        //        return View("Modal");
        //    }
        //    else
        //    {
        //        ViewBag.Titulo = "¡Error al eliminar!";
        //        ViewBag.Message = result.Message;
        //        return View("Modal");
        //    }


        //}

        [HttpGet]
        public JsonResult GetEstados(int idPais)
        {
            ML.Result resultEstados = BL.Estado.GetByIdPais(idPais);
            return Json(resultEstados.Objects);
        }

        [HttpGet]
        public JsonResult GetMunicipios(int idEstado)
        {
            ML.Result resultMunicipios = BL.Municipio.GetByIdEstado(idEstado);
            return Json(resultMunicipios.Objects);
        }

        [HttpGet]
        public JsonResult GetColonias(int idMunicipio)
        {
            ML.Result resultColonia = BL.Colonia.GetByIdMunicipio(idMunicipio);
            return Json(resultColonia.Objects);
        }

        public static byte[] imageToBytes(IFormFile image)
        {
            using var fileStream = image.OpenReadStream();
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            return bytes;
        }

        [HttpPost]
        public JsonResult CambiarStatus(int idUsuario, bool status)
        {
            ML.Result result = BL.Usuario.CambiarStatus(idUsuario, status);
            return Json(result);
        }

        //---------------------  Consumo de servicios de SL  ------------------------------

        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Nombre = usuario.Nombre == null ? "" : usuario.Nombre;
            usuario.ApellidoPaterno = usuario.ApellidoPaterno == null ? "" : usuario.ApellidoPaterno;

            ML.Result resultUsuarios = new ML.Result();
            resultUsuarios.Objects = new List<object>();

            using (HttpClient client = new HttpClient())
            {
                string webApi = configuration["WebApi"];
                client.BaseAddress = new Uri(webApi);

                var responseTask = client.GetAsync("GetAll");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var item in readTask.Result.Objects)
                    {
                        ML.Usuario usuarioList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(item.ToString());
                        resultUsuarios.Objects.Add(usuarioList);
                    }
                }
                usuario.Usuarios = resultUsuarios.Objects;
            }
            return View(usuario);
        }

        [HttpGet]
        public ActionResult Delete(int idUsuario)
        {
            ML.Result resultUsuario = new ML.Result();
            using (HttpClient client = new HttpClient())
            {
                string webApi = configuration["WebApi"];
                client.BaseAddress = new Uri(webApi);

                var postTask = client.GetAsync("delete/" + idUsuario);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ML.Usuario usuario = new ML.Usuario();
                    resultUsuario = BL.Usuario.GetAll(usuario);
                    return RedirectToAction("GetAll", resultUsuario);
                }
            }
            ML.Usuario usuario1 = new ML.Usuario();
            resultUsuario = BL.Usuario.GetAll(usuario1);
            return View("GetAll", resultUsuario);
        }

        //[HttpGet]
        //public static ML.Result GetById(int idUsuario)
        //{
        //    ML.Result result = new ML.Result();
        //    try
        //    {string webApi = System.Configuration.ConfigurationManager.AppSettings["WebApi"];
        //        
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(webApi);
        //            var responseTask = client.GetAsync("getbyid/" + idUsuario);
        //            responseTask.Wait();

        //            var resultApi = responseTask.Result;
        //            if (resultApi.IsSuccessStatusCode)
        //            {
        //                var readTask = resultApi.Content.ReadAsAsync<ML.Result>();
        //                readTask.Wait();
        //                ML.Usuario resulUsuario = new ML.Usuario();
        //                resulUsuario = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(readTask.Result.Object.ToString());
        //                result.Object = resulUsuario;
        //                result.Correct = true;
        //            }
        //            else
        //            {
        //                result.Correct = false;
        //                result.Message = "No existen registros con la tabla usuario";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Correct = false;
        //        result.Message = ex.Message;
        //    }
        //    return result;
        //}
    }
}
