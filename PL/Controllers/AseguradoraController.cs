using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class AseguradoraController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Aseguradora aseguradoraEmpty = new ML.Aseguradora();
            aseguradoraEmpty.Nombre = aseguradoraEmpty.Nombre == null ? "" : aseguradoraEmpty.Nombre;

            ML.Result resultAseguradoras = BL.Aseguradora.GetAll(aseguradoraEmpty);
            if (resultAseguradoras.Correct)
            {
                ML.Aseguradora aseguradora = new ML.Aseguradora();
                aseguradora.Aseguradoras = new List<object>();
                aseguradora.Aseguradoras = resultAseguradoras.Objects;
                return View(aseguradora);
            }
            else
            {
                ViewBag.Message = resultAseguradoras.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult GetAll(ML.Aseguradora aseguradora)
        {
            ML.Result result = BL.Aseguradora.GetAll(aseguradora);
            if (result.Correct)
            {
                aseguradora.Aseguradoras = new List<object>();
                aseguradora.Aseguradoras = result.Objects;
                return View(aseguradora);
            }
            else
            {
                ViewBag.Message = result.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult Form(int? idAseguradora)
        {
            ML.Aseguradora aseguradora = new ML.Aseguradora();
            aseguradora.Usuario = new ML.Usuario();
            aseguradora.Usuario.Usuarios = new List<object>();
            ML.Result resultUsuarios = BL.Usuario.GetAll(aseguradora.Usuario);
            aseguradora.Usuario.Usuarios = resultUsuarios.Objects;

            if (idAseguradora == null)
            {
                ViewBag.Titulo = "Agregar nueva aseguradora";
                ViewBag.Accion = "Agregar";
                return View(aseguradora);
            }
            else
            {
                ViewBag.Titulo = "Actualizar aseguradora";
                ViewBag.Accion = "Actualizar";
                ML.Result result = BL.Aseguradora.GetById(idAseguradora.Value);
                if (result.Object != null)
                {
                    aseguradora = ((ML.Aseguradora)result.Object);
                    aseguradora.Usuario.Usuarios = resultUsuarios.Objects;
                    return View(aseguradora);
                }
                else
                {
                    ViewBag.Message = result.Message;
                    return View("Modal");
                }
            }
        }

        [HttpPost]
        public ActionResult Form(ML.Aseguradora aseguradora)
        {
            if (ModelState.IsValid)
            {
                if (aseguradora.IdAseguradora == 0)
                {
                    var result = BL.Aseguradora.Add(aseguradora);
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
                    var result = BL.Aseguradora.Update(aseguradora);
                    if (result.Correct)
                    {
                        ViewBag.Titulo = "¡Modificación exitosa!";
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
                aseguradora.Usuario = new ML.Usuario();
                ML.Result resultUsuarios = BL.Usuario.GetAll(aseguradora.Usuario);

                aseguradora.Usuario.Usuarios = resultUsuarios.Objects;
                return View(aseguradora);
            }
        }

        [HttpPost]
        public JsonResult CambiarStatus(int idAseguradora, bool status)
        {
            ML.Result result = BL.Aseguradora.StatusUpdate(idAseguradora, status);
            return Json(result);
        }

        [HttpGet]
        public ActionResult Delete(int idAseguradora)
        {
            var result = BL.Aseguradora.Delete(idAseguradora);

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
    }
}
