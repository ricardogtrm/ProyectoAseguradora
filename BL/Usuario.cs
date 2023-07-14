using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ML;

namespace BL
{
    public class Usuario
    {
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {

                    int consulta = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.UserName}'" +
                        $",'{usuario.Nombre}','{usuario.ApellidoPaterno}','{usuario.ApellidoMaterno}'" +
                        $",'{usuario.Email}','{usuario.Password}','{usuario.Sexo}','{usuario.Telefono}'" +
                        $",'{usuario.Celular}','{usuario.FechaNacimiento}','{usuario.CURP}','{usuario.Imagen}'" +
                        $",{usuario.Rol.IdRol},'{usuario.Direccion.Calle}','{usuario.Direccion.NumeroInterior}'" +
                        $",'{usuario.Direccion.NumeroExterior}',{usuario.Direccion.Colonia.IdColonia}");
                    if (consulta > 0)
                    {
                        result.Message = "\nUsuario agregado exitosamente\n";
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "\nError: " + ex.Message;
            }
            return result;
        }

        public static ML.Result CambiarStatus(int idUsuario, bool status)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"UsuarioStatusUpdate {idUsuario},{status}");

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

        public static ML.Result Delete(int idUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    int consulta = context.Database.ExecuteSqlRaw($"UsuarioDelete {idUsuario}");
                    if (consulta > 0)
                    {
                        result.Correct = true;
                        result.Message = "\n--- Registro eliminado  ---\n";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "\nError: " + ex.Message;
            }
            return result;
        }

        public static ML.Result GetAll(ML.Usuario usuarios)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    var listaUsuarios = context.Usuarios.FromSqlRaw($"UsuarioGetAll '{usuarios.Nombre}','{usuarios.ApellidoPaterno}'").ToList();
                   
                    result.Objects = new List<object>();
                    if (listaUsuarios != null)
                    {
                        foreach (var columna in listaUsuarios)
                        {
                            ML.Usuario usuario = new ML.Usuario();
                            usuario.IdUsuario = columna.IdUsuario;
                            usuario.UserName = columna.UserName;
                            usuario.Nombre = columna.Nombre;
                            usuario.ApellidoPaterno = columna.ApellidoPaterno;
                            usuario.ApellidoMaterno = columna.ApellidoMaterno;
                            usuario.Email = columna.Email;
                            usuario.Password = columna.Password;
                            usuario.Sexo = columna.Sexo;
                            usuario.Telefono = columna.Telefono;
                            usuario.Celular = columna.Celular;
                            usuario.FechaNacimiento = columna.FechaNacimiento.Value.ToString("dd/MM/yyyy");
                            usuario.CURP = columna.Curp;
                            usuario.Imagen = columna.Imagen;
                            usuario.Status = columna.Status.Value;
                            //Información de ROl
                            usuario.Rol = new ML.Rol();
                            usuario.Rol.IdRol = (byte)(columna.IdRol);
                            usuario.Rol.Nombre = columna.RolUsuario;
                            //Información de Dirección
                            usuario.Direccion = new ML.Direccion();
                            usuario.Direccion.IdDireccion = columna.IdDireccion;
                            usuario.Direccion.Calle = columna.Calle;
                            usuario.Direccion.NumeroInterior = columna.NumeroInterior;
                            usuario.Direccion.NumeroExterior = columna.NumeroExterior;
                            //Información de Colonia
                            usuario.Direccion.Colonia = new ML.Colonia();
                            usuario.Direccion.Colonia.IdColonia = columna.IdColonia;
                            usuario.Direccion.Colonia.Nombre = columna.NombreColonia;
                            usuario.Direccion.Colonia.CodigoPostal = columna.CodigoPostal;
                            //Información de Municipio
                            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuario.Direccion.Colonia.Municipio.IdMunicipio = columna.IdMunicipio;
                            usuario.Direccion.Colonia.Municipio.Nombre = columna.NombreMunicipio;
                            //Información de Estado
                            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                            usuario.Direccion.Colonia.Municipio.Estado.IdEstado = columna.IdEstado;
                            usuario.Direccion.Colonia.Municipio.Estado.Nombre = columna.NombreEstado;
                            //Información de Pais
                            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = columna.IdPais;
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = columna.NombrePais;

                            result.Objects.Add(usuario);
                        }
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "\nError: " + ex.Message;
            }
            return result;
        }

        public static ML.Result GetById(int idUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    var columna = context.Usuarios.FromSqlRaw($"UsuarioGetById {idUsuario}").AsEnumerable().FirstOrDefault();
                    if (columna != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.UserName = columna.UserName;
                        usuario.Nombre = columna.Nombre;
                        usuario.ApellidoPaterno = columna.ApellidoPaterno;
                        usuario.ApellidoMaterno = columna.ApellidoMaterno;
                        usuario.Email = columna.Email;
                        usuario.Password = columna.Password;
                        usuario.Sexo = columna.Sexo;
                        usuario.Telefono = columna.Telefono;
                        usuario.Celular = columna.Celular;
                        usuario.FechaNacimiento = columna.FechaNacimiento.Value.ToString("dd/MM/yyyy");
                        usuario.CURP = columna.Curp;
                        usuario.Imagen = columna.Imagen;
                        usuario.Status = columna.Status.Value;
                        //Información de Rol
                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = (byte)(columna.IdRol);
                        usuario.Rol.Nombre = columna.RolUsuario;
                        //Información de Dirección
                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.IdDireccion = columna.IdDireccion;
                        usuario.Direccion.Calle = columna.Calle;
                        usuario.Direccion.NumeroInterior = columna.NumeroInterior;
                        usuario.Direccion.NumeroExterior = columna.NumeroExterior;
                        //Información de Colonia
                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.IdColonia = columna.IdColonia;
                        usuario.Direccion.Colonia.Nombre = columna.NombreColonia;
                        usuario.Direccion.Colonia.CodigoPostal = columna.CodigoPostal;
                        //Información de Municipio
                        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario.Direccion.Colonia.Municipio.IdMunicipio = columna.IdMunicipio;
                        usuario.Direccion.Colonia.Municipio.Nombre = columna.NombreMunicipio;
                        //Información de Estado
                        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                        usuario.Direccion.Colonia.Municipio.Estado.IdEstado = columna.IdEstado;
                        usuario.Direccion.Colonia.Municipio.Estado.Nombre = columna.NombreEstado;
                        //Información de Pais
                        usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = columna.IdPais;
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = columna.NombrePais;

                        result.Object = usuario;
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "\nError: " + result.Ex.Message;
            }
            return result;
        }

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.RGutierrezProgramacionNCapasContext context = new DL.RGutierrezProgramacionNCapasContext())
                {
                    int consulta = context.Database.ExecuteSqlRaw($"UsuarioUpdate {usuario.IdUsuario}" +
                        $",'{usuario.UserName}','{usuario.Nombre}','{usuario.ApellidoPaterno}'" +
                        $",'{usuario.ApellidoMaterno}','{usuario.Email}','{usuario.Password}'" +
                        $",'{usuario.Sexo}','{usuario.Telefono}','{usuario.Celular}'" +
                        $",'{usuario.FechaNacimiento}','{usuario.CURP}','{usuario.Imagen}'" +
                        $",{usuario.Rol.IdRol},'{usuario.Direccion.Calle}','{usuario.Direccion.NumeroInterior}'" +
                        $",'{usuario.Direccion.NumeroExterior}',{usuario.Direccion.Colonia.IdColonia}");

                    if (consulta > 0)
                    {
                        result.Message = "\n---  Registro modificado  ---\n";
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "\nError: " + result.Ex.Message;
            }
            return result;
        }

        public static ML.Result ConvertExcelToDataTable(string connString)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (OleDbConnection context = new OleDbConnection(connString))
                {
                    string query = "SELECT * FROM [Hoja1$]";
                    using (OleDbCommand command = new OleDbCommand())
                    {
                        command.Connection = context;
                        command.CommandText = query;

                        OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
                        dataAdapter.SelectCommand = command;

                        DataTable tablaUsuario = new DataTable();
                        dataAdapter.Fill(tablaUsuario);

                        if (tablaUsuario.Rows.Count > 0)
                        {
                            result.Objects = new List<object>();
                            foreach (DataRow fila in tablaUsuario.Rows)
                            {
                                ML.Usuario usuario = new ML.Usuario();

                                usuario.UserName = fila[0].ToString();
                                usuario.Nombre = fila[1].ToString();
                                usuario.ApellidoPaterno = fila[2].ToString();
                                usuario.ApellidoMaterno = fila[3].ToString();
                                usuario.Email = fila[4].ToString();
                                usuario.Password = fila[5].ToString();
                                usuario.Sexo = fila[6].ToString();
                                usuario.Telefono = fila[7].ToString();
                                usuario.Celular = fila[8].ToString();
                                usuario.FechaNacimiento = fila[9].ToString();
                                usuario.CURP = fila[10].ToString();
                                usuario.Rol = new ML.Rol();
                                //Información de Rol
                                int fila11 = int.Parse(fila[11].ToString());
                                usuario.Rol.IdRol = (byte)(fila11);
                                //Información de Dirección
                                usuario.Direccion = new ML.Direccion();
                                usuario.Direccion.Calle = fila[12].ToString();
                                usuario.Direccion.NumeroInterior = fila[13].ToString();
                                usuario.Direccion.NumeroExterior = fila[14].ToString();
                                //Información de Colonia
                                usuario.Direccion.Colonia = new ML.Colonia();
                                usuario.Direccion.Colonia.IdColonia = int.Parse(fila[15].ToString());

                                result.Objects.Add(usuario);
                            }
                            result.Correct = true;
                        }
                        result.Object = tablaUsuario;
                        if (tablaUsuario.Rows.Count > 1)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.Message = "El archivo esta vácio";
                        }
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

        public static ML.Result ValidarExcel(List<object> Objects)
        {
            ML.Result result = new ML.Result();
            try
            {
                result.Objects = new List<object>();
                int i = 1;
                foreach (ML.Usuario usuario in Objects)
                {
                    ML.ErrorExcel error = new ML.ErrorExcel();
                    error.IdRegistro = i++;
                    //Información del usuario
                    usuario.UserName = (usuario.UserName == "") ? error.Mensaje += "Nombre de usuario es requerido " : usuario.UserName;
                    usuario.Nombre = (usuario.Nombre == "") ? error.Mensaje += "Nombre es requerido " : usuario.Nombre;
                    usuario.ApellidoPaterno = (usuario.ApellidoPaterno == "") ? error.Mensaje += "Apellido paterno es requerido " : usuario.ApellidoPaterno;
                    usuario.ApellidoMaterno = (usuario.ApellidoMaterno == "") ? error.Mensaje += "Apellido materno es requerido " : usuario.ApellidoMaterno;
                    usuario.Email = (usuario.Email == "") ? error.Mensaje += "El nombre es requerido " : usuario.Email;
                    usuario.Password = (usuario.Password == "") ? error.Mensaje += "El nombre es requerido " : usuario.Password;
                    usuario.Sexo = (usuario.Sexo == "") ? error.Mensaje += "El nombre es requerido " : usuario.Sexo;
                    usuario.Telefono = (usuario.Telefono == "") ? error.Mensaje += "El nombre es requerido " : usuario.Telefono;
                    //Información de la dirección
                    usuario.Direccion = new ML.Direccion();
                    usuario.Direccion.Calle = (usuario.Direccion.Calle == "") ? error.Mensaje += "Calle es requerida " : usuario.Direccion.Calle;
                    usuario.Direccion.NumeroInterior = (usuario.Direccion.NumeroInterior == "") ? error.Mensaje += "Numero interior es requerido " : usuario.Direccion.NumeroInterior;
                    usuario.Direccion.NumeroExterior = (usuario.Direccion.NumeroExterior == "") ? error.Mensaje += "Numero exterior es requerido " : usuario.Direccion.NumeroExterior;
                    usuario.Direccion.Colonia = new ML.Colonia();
                    //usuario.Direccion.Colonia.IdColonia = (usuario.Direccion.Colonia.IdColonia == 0) ? error.Mensaje += "Id colonia es requerido " : usuario.Direccion.Colonia.IdColonia;

                    if (error.Mensaje != null)
                    {
                        result.Objects.Add(error);
                    }
                }
                result.Correct = true;
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
