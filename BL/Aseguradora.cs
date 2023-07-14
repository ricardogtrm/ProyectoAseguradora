using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Aseguradora
    {
        public static ML.Result Add(ML.Aseguradora aseguradora)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"AseguradoraAdd '{aseguradora.Nombre}'" +
                        $",{aseguradora.Usuario.IdUsuario}");
                    if (query > 0)
                    {
                        result.Correct = true;
                        result.Message = "¡Aseguradora registrada!";
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

        public static ML.Result StatusUpdate(int IdAseguradora, bool status)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"AseguradoraStatusUpdate {IdAseguradora},{status}");
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

        public static ML.Result Delete(int idAseguradora)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"AseguradoraDelete {idAseguradora}");
                    if (query > 0)
                    {
                        result.Correct = true;
                        result.Message = "¡Aseguradora eliminada!";
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

        public static ML.Result GetAll(ML.Aseguradora aseguradoraObj)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    var listAseguradora = context.Aseguradoras.FromSqlRaw($"AseguradoraGetAll '{aseguradoraObj.Nombre}'").ToList();
                    if (listAseguradora != null && listAseguradora.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var obj in listAseguradora)
                        {
                            ML.Aseguradora aseguradora = new ML.Aseguradora();
                            aseguradora.IdAseguradora = obj.IdAseguradora;
                            aseguradora.Nombre = obj.Nombre;
                            aseguradora.FechaCreacion = obj.FechaCreacion.ToString();
                            aseguradora.FechaModificacion = obj.FechaModificacion.ToString();
                            aseguradora.Status = obj.Status.Value;
                            //Información del usuario
                            aseguradora.Usuario = new ML.Usuario();
                            aseguradora.Usuario.IdUsuario = obj.IdUsuario.Value;
                            aseguradora.Usuario.Nombre = obj.NombreUsuario;
                            aseguradora.Usuario.ApellidoPaterno = obj.ApellidoPaterno;
                            aseguradora.Usuario.ApellidoMaterno = obj.ApellidoMaterno;
                            result.Objects.Add(aseguradora);
                        }
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

        public static ML.Result GetById(int idAseguradora)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    var query = context.Aseguradoras.FromSqlRaw($"AseguradoraGetById {idAseguradora}").AsEnumerable().FirstOrDefault();
                    if (query != null)
                    {
                        ML.Aseguradora aseguradora = new ML.Aseguradora();
                        aseguradora.IdAseguradora = query.IdAseguradora;
                        aseguradora.Nombre = query.Nombre;
                        aseguradora.FechaCreacion = query.FechaCreacion.ToString();
                        aseguradora.FechaModificacion = query.FechaModificacion.ToString();
                        aseguradora.Status = query.Status.Value;
                        aseguradora.Usuario = new ML.Usuario();
                        aseguradora.Usuario.IdUsuario = query.IdUsuario.Value;
                        result.Object = aseguradora;
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

        public static ML.Result Update(ML.Aseguradora aseguradora)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"AseguradoraUpdate {aseguradora.IdAseguradora}" +
                        $",'{aseguradora.Nombre}',{aseguradora.Usuario.IdUsuario}");
                    if (query > 0)
                    {
                        result.Correct = true;
                        result.Message = "¡Registro modificado!";
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
    }
}
