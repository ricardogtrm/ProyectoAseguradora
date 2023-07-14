using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Municipio
    {
        public static ML.Result GetByIdEstado(int idEstado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    var municipiosLista = context.Municipios.FromSqlRaw($"MunicipioGetByIdEstado {idEstado}").ToList();
                    if (municipiosLista != null && municipiosLista.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var rowMunicipio in municipiosLista)
                        {
                            ML.Municipio municipio = new ML.Municipio();
                            municipio.IdMunicipio = rowMunicipio.IdMunicipio;
                            municipio.Nombre = rowMunicipio.Nombre;
                            municipio.Estado = new ML.Estado();
                            municipio.Estado.IdEstado = rowMunicipio.IdMunicipio;

                            result.Objects.Add(municipio);
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
