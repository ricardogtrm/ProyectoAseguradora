using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Pais
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    var paisesLista = context.Pais.FromSqlRaw("PaisGetAll").ToList();
                    if (paisesLista != null && paisesLista.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var rowPais in paisesLista)
                        {
                            ML.Pais pais = new ML.Pais();
                            pais.IdPais = rowPais.IdPais;
                            pais.Nombre = rowPais.Nombre;

                            result.Objects.Add(pais);
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
