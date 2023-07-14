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
                return View();
            }
        }

        [HttpPost]
        public ActionResult Form(ML.Empleado empleado)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Delete(string numeroEmpleado)
        {
            return View();
        }

        public JsonResult CambiarStatus(string numeroEmpleado, bool status)
        {
            ML.Result result = BL.Empleado.StatusUpdate(numeroEmpleado, status);
            return Json(result);
        }
    }
}
