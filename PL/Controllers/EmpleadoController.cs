using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class EmpleadoController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Empleado empleado = new ML.Empleado();
            empleado.Empresa = new ML.Empresa();
            empleado.Empresa.IdEmpresa = empleado.Empresa.IdEmpresa == null ? 0 : empleado.Empresa.IdEmpresa;
            empleado.Nombre = empleado.Nombre == null ? "" : empleado.Nombre;

            ML.Result result = BL.Empleado.GetAll(empleado);

            if (result.Correct)
            {
                empleado.Empleados = new List<object>();
                empleado.Empleados = result.Objects;
                return View(empleado);
            }
            else
            {
                ViewBag.Message = result.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult GetAll(ML.Empleado empleado)
        {
            ML.Result result = BL.Empleado.GetAll(empleado);
            if (result.Correct)
            {
                empleado.Empleados = new List<object>();
                empleado.Empleados = result.Objects;
                return View(empleado);
            }
            else
            {
                ViewBag.Message = result.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult Form(string? numeroEmpleado)
        {
            ML.Result resultEmpresa = BL.Empresa.GetAll();
            ML.Empleado empleado = new ML.Empleado();
            empleado.Empresa = new ML.Empresa();

            empleado.Empresa.Empresas = resultEmpresa.Objects;

            if (numeroEmpleado == null)
            {
                ViewBag.Titulo = "Agregar nuevo empleado";
                ViewBag.Accion = "Agregar";
                return View(empleado);
            }
            else
            {
                ViewBag.Titulo = "Actualizar empleado";
                ViewBag.Accion = "Actualizar";
                ML.Result result = BL.Empleado.GetById(numeroEmpleado);
                if (result.Object != null)
                {
                    empleado = ((ML.Empleado)result.Object);
                    ML.Result resultEmpresas = BL.Empresa.GetAll();
                    empleado.Empresa.Empresas = resultEmpresas.Objects;
                    return View(empleado);
                }
                else
                {
                    ViewBag.Message = result.Message;
                    return View("Modal");
                }
            }
        }

        [HttpPost]
        public ActionResult Form(ML.Empleado empleado)
        {
            //if (ModelState.IsValid)
            //{
            IFormFile image = Request.Form.Files["fileImage"];
            if (image != null)
            {
                byte[] imageBytes = imageToBytes(image);
                empleado.Foto = Convert.ToBase64String(imageBytes);
            }
            if (empleado.NumeroEmpleado == "")
            {
                var result = BL.Empleado.Add(empleado);
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
                var result = BL.Empleado.Update(empleado);
                if (result.Correct)
                {
                    ViewBag.Titulo = "¡Cambios realizados con exitoso!";
                    ViewBag.Message = result.Message;
                    return View("Modal");
                }
                else
                {
                    ViewBag.Titulo = "¡Error al modificar!";
                    ViewBag.Message = result.Message;
                    return View("Modal");
                }
            }
            //}
            //else
            //{
            //ML.Result resultEmpresa = BL.Empresa.GetAll();
            //empleado.Empresa.Empresas = resultEmpresa.Objects;
            //}
            return View();
        }

        [HttpGet]
        public ActionResult Delete(string numeroEmpleado)
        {
            var result = BL.Empleado.Delete(numeroEmpleado);

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

        public JsonResult CambiarStatus(string numeroEmpleado, bool status)
        {
            ML.Result result = BL.Empleado.StatusUpdate(numeroEmpleado, status);
            return Json(result);
        }

        public static byte[] imageToBytes(IFormFile image)
        {
            using var fileStream = image.OpenReadStream();
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            return bytes;
        }

    }
}
