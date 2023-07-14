using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Empresa
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    var listaEmpresas = context.Empresas.FromSqlRaw("EmpresaGetAll").ToList();

                    if (listaEmpresas != null && listaEmpresas.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var filaEmpresa in listaEmpresas)
                        {
                            ML.Empresa empresa = new ML.Empresa();
                            empresa.IdEmpresa = filaEmpresa.IdEmpresa;
                            empresa.Nombre = filaEmpresa.Nombre;
                            empresa.Telefono = filaEmpresa.Telefono;
                            empresa.Email = filaEmpresa.Email;
                            empresa.DireccionWeb = filaEmpresa.DireccionWeb;
                            //empresa.Logo = filaEmpresa.Logo;

                            result.Objects.Add(empresa);

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
    }
}
