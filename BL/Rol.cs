using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Rol
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    var roles = context.Rols.FromSqlRaw("RolGetAll").ToList();
                    result.Objects = new List<object>();

                    if (roles.Count != 0 && roles != null)
                    {
                        foreach (var dato in roles)
                        {
                            ML.Rol rol = new ML.Rol();
                            rol.IdRol = dato.IdRol;
                            rol.Nombre = dato.Nombre;

                            result.Objects.Add(rol);
                        }
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = ex.Message;
            }
            return result;
        }

    }
}
