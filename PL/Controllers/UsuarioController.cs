using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            //usuario.Nombre = "";
            //usuario.ApellidoPaterno = "";
            usuario.Nombre = usuario.Nombre == null ? "" : usuario.Nombre;
            usuario.ApellidoPaterno = usuario.ApellidoPaterno == null ? "" : usuario.ApellidoPaterno;

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
                ML.Result result = BL.Usuario.GetById(idUsuario.Value);

                if (result.Object != null)
                {
                    usuario = ((ML.Usuario)result.Object);
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
                    ViewBag.Message = result.Message;
                    return View("Modal");
                }
            }
        }

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                IFormFile image = Request.Form.Files["fileImage"];
                if (image != null)
                {
                    byte[] imageBytes = imageToBytes(image);
                    usuario.Imagen = Convert.ToBase64String(imageBytes);
                }
                if (usuario.IdUsuario == 0)
                {
                    var result = BL.Usuario.Add(usuario);
                    if (result.Correct)
                    {
                        ViewBag.Titulo = "¡Registro exitoso!";
                        ViewBag.Message = result.Message;
                        return View("Modal");
                    }
                    else
                    {
                        ViewBag.Titulo = "¡Error al registrar!";
                        ViewBag.Message = result.Message;
                        return View("Modal");
                    }
                }
                else
                {
                    var result = BL.Usuario.Update(usuario);
                    if (result.Correct)
                    {
                        ViewBag.Titulo = "¡Registro exitoso!";
                        ViewBag.Message = result.Message;
                        return View("Modal");
                    }
                    else
                    {
                        ViewBag.Titulo = "¡Error al registrar!";
                        ViewBag.Message = result.Message;
                        return View("Modal");
                    }
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

        [HttpGet]
        public ActionResult Delete(int idUsuario)
        {
            var result = BL.Usuario.Delete(idUsuario);

            if (result.Correct)
            {
                ViewBag.Titulo = "¡Registro eliminado!";
                ViewBag.Message = result.Message;
                return View("Modal");
            }
            else
            {
                ViewBag.Titulo = "¡Error al eliminar!";
                ViewBag.Message = result.Message;
                return View("Modal");
            }


        }

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
    }
}
