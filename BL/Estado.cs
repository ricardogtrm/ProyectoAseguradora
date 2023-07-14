using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Estado
    {
        public static ML.Result GetByIdPais(int idPais)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    var estadosLista = context.Estados.FromSqlRaw($"EstadoGetByIdPais {idPais}").ToList();
                    if (estadosLista != null && estadosLista.Count > 0) 
                    {
                        result.Objects = new List<object>();
                        foreach (var rowEstado in estadosLista)
                        {
                            ML.Estado estado = new ML.Estado();
                            estado.IdEstado = rowEstado.IdEstado;
                            estado.Nombre = rowEstado.Nombre;
                            estado.Pais = new ML.Pais();
                            estado.Pais.IdPais = rowEstado.IdEstado;

                            result.Objects.Add(estado);
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
