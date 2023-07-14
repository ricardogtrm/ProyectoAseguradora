using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Colonia
    {
        public static ML.Result GetByIdMunicipio(int idMunicipio)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    var coloniaLista = context.Colonia.FromSqlRaw($"ColoniaGetByIdMunicipio {idMunicipio}").ToList();
                    if (coloniaLista != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var rowColonia in coloniaLista)
                        {
                            ML.Colonia colonia = new ML.Colonia();
                            colonia.IdColonia = rowColonia.IdColonia;
                            colonia.Nombre = rowColonia.Nombre;
                            colonia.CodigoPostal = rowColonia.CodigoPostal;
                            colonia.Municipio = new ML.Municipio();
                            colonia.Municipio.IdMunicipio = rowColonia.IdMunicipio.Value;

                            result.Objects.Add(colonia);
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