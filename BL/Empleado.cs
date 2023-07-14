using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Empleado
    {
        public static ML.Result Add(ML.Empleado empleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    int filasAfectadas = context.Database.ExecuteSqlRaw($"EmpleadoAdd '{empleado.NumeroEmpleado}'" +
                        $",'{empleado.RFC}','{empleado.Nombre}','{empleado.ApellidoPaterno}','{empleado.ApellidoMaterno}'" +
                        $",'{empleado.Email}','{empleado.Telefono}','{empleado.FechaNacimiento}','{empleado.NSS}'" +
                        $",'{empleado.FechaIngreso}','{empleado.Foto}',{empleado.Empresa.IdEmpresa}");
                    if (filasAfectadas > 0)
                    {
                        result.Correct = true;
                        result.Message = "¡El empleado se registro correctamente!";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public static ML.Result Delete(string numeroEmpleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"EmpleadoDelete '{numeroEmpleado}'");
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = true;
                result.Message = ex.Message;
            }
            return result;
        }

        public static ML.Result GetAll(ML.Empleado empleados)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    var listaEmpleados = context.Empleados.FromSqlRaw($"EmpleadoGetAll '{empleados.Empresa.IdEmpresa}','{empleados.Nombre}'").ToList();

                    result.Objects = new List<object>();
                    if (listaEmpleados != null && listaEmpleados.Count > 0)
                    {
                        foreach (var filaEmpleado in listaEmpleados)
                        {
                            ML.Empleado empleado = new ML.Empleado();
                            empleado.NumeroEmpleado = filaEmpleado.NumeroEmpleado;
                            empleado.RFC = filaEmpleado.Rfc;
                            empleado.Nombre = filaEmpleado.Nombre;
                            empleado.ApellidoPaterno = filaEmpleado.ApellidoPaterno;
                            empleado.ApellidoMaterno = filaEmpleado.ApellidoMaterno;
                            empleado.Email = filaEmpleado.Email;
                            empleado.Telefono = filaEmpleado.Telefono;
                            empleado.FechaNacimiento = filaEmpleado.FechaNacimiento.ToString();
                            empleado.NSS = filaEmpleado.Nss;
                            empleado.FechaIngreso = filaEmpleado.FechaIngreso.ToString();
                            empleado.Foto = filaEmpleado.Foto;
                            empleado.Status = filaEmpleado.Status.Value;
                            //Información de Empresa
                            empleado.Empresa = new ML.Empresa();
                            empleado.Empresa.IdEmpresa = filaEmpleado.IdEmpresa.Value;
                            empleado.Empresa.Nombre = filaEmpleado.NombreEmpresa;

                            result.Objects.Add(empleado);

                        }
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public static ML.Result GetById(string numeroEmpleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    var empleadoObj = context.Empleados.FromSqlRaw($"EmpleadoGetById {numeroEmpleado}").AsEnumerable().FirstOrDefault();

                    if (empleadoObj != null)
                    {
                        ML.Empleado empleado = new ML.Empleado();
                        empleado.NumeroEmpleado = empleadoObj.NumeroEmpleado;
                        empleado.RFC = empleadoObj.Rfc;
                        empleado.Nombre = empleadoObj.Nombre;
                        empleado.ApellidoPaterno = empleadoObj.ApellidoPaterno;
                        empleado.ApellidoMaterno = empleadoObj.ApellidoMaterno;
                        empleado.Email = empleadoObj.Email;
                        empleado.Telefono = empleadoObj.Telefono;
                        empleado.FechaNacimiento = empleadoObj.FechaNacimiento.ToString();
                        empleado.NSS = empleadoObj.Nss;
                        empleado.FechaIngreso = empleadoObj.FechaIngreso.ToString();
                        empleado.Foto = empleadoObj.Foto;
                        empleado.Status = empleadoObj.Status.Value;
                        //Información de Empresa
                        empleado.Empresa = new ML.Empresa();
                        empleado.Empresa.IdEmpresa = empleadoObj.IdEmpresa.Value;
                        empleado.Empresa.Nombre = empleadoObj.NombreEmpresa;
                        result.Object = empleado;
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public static ML.Result StatusUpdate(string numeroEmpleado, bool status)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"EmpleadoStatusUpdate {status},'{numeroEmpleado}'");
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public static ML.Result Update(ML.Empleado empleado)
        {
            ML.Result result = new ML.Result();
            try
            {

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }
            return result;
        }

    }
}
