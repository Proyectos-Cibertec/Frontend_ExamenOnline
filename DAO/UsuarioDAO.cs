using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using BE;
using Utilitarios;

namespace DAO
{
    public class UsuarioDAO
    {
        private Conexion conexion = new Conexion();

        public UsuarioBE ObtenerUsuario(string usuario)
        {
            UsuarioBE oUsuario = null;
            try
            {
                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Usp_ObtenerUsuarioPorCuenta", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Usuario", usuario);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                oUsuario = new UsuarioBE();
                                oUsuario.Rol = new RolBE();

                                oUsuario.IdUsuario = dr.GetInt32(0);
                                oUsuario.Nombres = dr.GetString(1);
                                oUsuario.ApellidoPaterno = dr.GetString(2);
                                oUsuario.ApellidoMaterno = dr.GetString(3);
                                oUsuario.Correo = dr.GetString(4);
                                oUsuario.Dni = dr.GetString(5);
                                oUsuario.Usuario = dr.GetString(6);
                                oUsuario.Contraseña = Convert.ToString(dr.GetString(7));
                                oUsuario.imgData = Convert.ToString(dr.GetString(8));
                                oUsuario.Intentos = dr.GetInt32(9);
                                oUsuario.Bloqueado = dr.GetBoolean(10);
                                oUsuario.Rol.IdRol = Convert.ToInt32(dr["IdRol"]);
                                oUsuario.Rol.NombreRol = Convert.ToString(dr["NombreRol"]);
                            }
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex) {}

            return oUsuario;
        }

        public UsuarioBE IniciarSesion(UsuarioBE usuario)
        {
            UsuarioBE oUsuario = null;
            try
            {
                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Usp_Login", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                        cmd.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                oUsuario = new UsuarioBE();
                                oUsuario.Rol = new RolBE();
                                oUsuario.TipoUsuario = new TipoUsuarioBE();

                                oUsuario.IdUsuario = dr["IdUsuario"] is DBNull ? 0 : Convert.ToInt32(dr["IdUsuario"]);
                                oUsuario.Nombres = dr["Nombres"] is DBNull ? string.Empty : dr["Nombres"].ToString();
                                oUsuario.ApellidoPaterno = dr["ApellidoPaterno"] is DBNull ? string.Empty : dr["ApellidoPaterno"].ToString();
                                oUsuario.ApellidoMaterno = dr["ApellidoMaterno"] is DBNull ? string.Empty : dr["ApellidoMaterno"].ToString();
                                oUsuario.Correo = dr["Correo"] is DBNull ? string.Empty : dr["Correo"].ToString();
                                oUsuario.Dni = dr["Dni"] is DBNull ? string.Empty : dr["Dni"].ToString();
                                oUsuario.Usuario = dr["Usuario"] is DBNull ? string.Empty : dr["Usuario"].ToString();
                                oUsuario.Contraseña = dr["Contraseña"] is DBNull ? string.Empty : dr["Contraseña"].ToString();
                                oUsuario.imgData = dr["imagen"] is DBNull ? string.Empty : dr["imagen"].ToString();
                                oUsuario.FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]);
                                oUsuario.Intentos = dr["Intentos"] is DBNull ? 0 : Convert.ToInt32(dr["Intentos"]);
                                oUsuario.Bloqueado = dr["Bloqueado"] is DBNull ? false : Convert.ToBoolean(dr["Bloqueado"]);
                                oUsuario.TipoUsuario.IdTipoUsuario = dr["IdTipoUsuario"] is DBNull ? 0 : Convert.ToInt32(dr["IdTipoUsuario"]);
                                oUsuario.TipoUsuario.Descripcion = dr["Descripcion"] is DBNull ? string.Empty : Convert.ToString(dr["Descripcion"]);
                                oUsuario.Rol.IdRol = dr["IdRol"] is DBNull ? 0 : Convert.ToInt32(dr["IdRol"]);
                                oUsuario.Rol.NombreRol = dr["NombreRol"] is DBNull ? string.Empty : Convert.ToString(dr["NombreRol"]);
                            }
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex) {}
            return oUsuario;
        }

        public RespuestaBE RegistrarUsuario(UsuarioBE usuario)
        {
            RespuestaBE respuesta = new RespuestaBE();
            byte[] imagenOriginal = new byte[0];
            string ImagenDataURL64 = "";
            if (usuario.imgUsuario != null)
            {
                int tamanio = usuario.imgUsuario.ContentLength;
                imagenOriginal = new byte[tamanio];
                usuario.imgUsuario.InputStream.Read(imagenOriginal, 0, tamanio);

                System.Drawing.Bitmap ImagenOriginalBinaria = new System.Drawing.Bitmap(usuario.imgUsuario.InputStream);
                ImagenDataURL64 = "data:image/png;base64," + Convert.ToBase64String(imagenOriginal);
            }

            respuesta.Registra = false;

            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("USP_MANTENIMIENTO_USUARIO", con))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUsuario", "");
                        cmd.Parameters.AddWithValue("@Nombres", usuario.Nombres);
                        cmd.Parameters.AddWithValue("@ApellidoPaterno", usuario.ApellidoPaterno);
                        cmd.Parameters.AddWithValue("@ApellidoMaterno", usuario.ApellidoMaterno);
                        cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                        cmd.Parameters.AddWithValue("@Dni", usuario.Dni);
                        cmd.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                        cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasenia);
                        cmd.Parameters.AddWithValue("@imagen", ImagenDataURL64);
                        cmd.Parameters.AddWithValue("@Intentos", 3);
                        cmd.Parameters.AddWithValue("@Bloqueado", 0);
                        cmd.Parameters.AddWithValue("@IdTipoUsuario", Constantes.TipoUsuario.FREE); // 0: Usuario FREE por defecto
                        cmd.Parameters.AddWithValue("@Estado", 1);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 1);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        respuesta.Registra = true;
                    }
                    catch (SqlException ex)
                    {
                        respuesta.Registra = false;
                        respuesta.MensajeError = ex.Message;
                        respuesta.CodigoError = ex.Number;
                    }
                    catch (Exception ex)
                    {
                        respuesta.Registra = false;
                        respuesta.MensajeError = ex.Message;
                        respuesta.CodigoError = -1;
                    }
                    finally
                    {
                        con.Close();
                    }

                }

            }
            return respuesta;
        }

    }
}
