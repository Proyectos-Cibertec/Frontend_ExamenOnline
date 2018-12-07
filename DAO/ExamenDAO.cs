using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.Data.SqlClient;
using System.Data;

namespace DAO
{
    public class ExamenDAO
    {
        Conexion conexion = new Conexion();

        public RespuestaBE RegistrarExamen(ExamenBE examen)
        {
            RespuestaBE respuesta = new RespuestaBE();
            respuesta.Registra = false;

            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand cmd1 = new SqlCommand("Usp_InsertarExamen", con, tr))
                {
                    try
                    {
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                        cmd1.Parameters.AddWithValue("@FechaExpiracion", examen.FechaExpiracion);
                        cmd1.Parameters.AddWithValue("@IdUsuario", examen.Usuario.IdUsuario);
                        cmd1.Parameters.AddWithValue("@Titulo", examen.Titulo);
                        cmd1.Parameters.AddWithValue("@Descripcion", examen.Descripcion);
                        cmd1.Parameters.AddWithValue("@TiempoMaximo", examen.TiempoMaximo);
                        cmd1.Parameters.AddWithValue("@IdAsignatura", examen.Asignatura.IdAsignatura);
                        cmd1.Parameters.AddWithValue("@Clave", examen.Clave);

                        // Se obtiene el Id generado del examen
                        int idExamen = Convert.ToInt32(cmd1.ExecuteScalar());

                        // Se registra las preguntas del examen
                        foreach (var pregunta in examen.LstPreguntas)
                        {
                            SqlCommand cmd2 = new SqlCommand("Usp_InsertarPregunta", con, tr);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@Pregunta", pregunta.Pregunta);
                            cmd2.Parameters.AddWithValue("@Numero", pregunta.Numero);
                            cmd2.Parameters.AddWithValue("@IdExamen", idExamen);

                            // Se obtiene el Id generado de cada pregunta
                            int idPregunta = Convert.ToInt32(cmd2.ExecuteScalar());

                            if (pregunta.LstAlternativa != null)
                            {
                                // Se registran las alternativas para cada pregunta
                                foreach (var alternativa in pregunta.LstAlternativa)
                                {
                                    SqlCommand cmd3 = new SqlCommand("Usp_InsertarAlternativa", con, tr);
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    cmd3.Parameters.AddWithValue("@IdPregunta", idPregunta);
                                    cmd3.Parameters.AddWithValue("@Numero", alternativa.Numero);
                                    cmd3.Parameters.AddWithValue("@Descripcion", alternativa.Descripcion);
                                    cmd3.Parameters.AddWithValue("@OpcionCorrecta", alternativa.OpcionCorrecta);
                                    cmd3.ExecuteNonQuery();
                                }
                            }

                            if (pregunta.LstImagen != null)
                            {
                                // Se registran las imágenes para cada pregunta
                                foreach (var imagen in pregunta.LstImagen)
                                {
                                    SqlCommand cmd4 = new SqlCommand("Usp_InsertarImagen", con, tr);
                                    cmd4.CommandType = CommandType.StoredProcedure;
                                    cmd4.Parameters.AddWithValue("@Imagen", imagen.Imagen);
                                    cmd4.Parameters.AddWithValue("@IdPregunta", idPregunta);
                                    cmd4.ExecuteNonQuery();
                                }
                            }

                            if (pregunta.LstVideo != null)
                            {
                                // Se registran los videos para cada pregunta
                                foreach (var video in pregunta.LstVideo)
                                {
                                    SqlCommand cmd5 = new SqlCommand("Usp_InsertarVideo", con, tr);
                                    cmd5.CommandType = CommandType.StoredProcedure;
                                    cmd5.Parameters.AddWithValue("@Video", video.Video);
                                    cmd5.Parameters.AddWithValue("@IdPregunta", idPregunta);
                                    cmd5.ExecuteNonQuery();
                                }
                            }
                        }

                        tr.Commit();
                        respuesta.Registra = true;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        respuesta.MensajeError = ex.Message;
                        respuesta.Registra = false;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return respuesta;
        }

        public ExamenBE ObtenerExamen(int IdExamen)
        {
            ExamenBE objExamen = null;
            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ObtenerExamen", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdExamen", IdExamen);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objExamen = new ExamenBE();
                            objExamen.Usuario = new UsuarioBE();
                            objExamen.Asignatura = new AsignaturaBE();

                            objExamen.IdExamen = dr["IdExamen"] is DBNull ? 0 : Convert.ToInt32(dr["IdExamen"]);
                            objExamen.FechaRegString = Convert.ToDateTime(dr["FechaRegistro"]).ToShortDateString();
                            objExamen.FechaExpString = Convert.ToDateTime(dr["FechaExpiracion"]).ToShortDateString();
                            objExamen.Titulo = dr["Titulo"] is DBNull ? string.Empty : dr["Titulo"].ToString();
                            objExamen.Descripcion = dr["Descripcion"] is DBNull ? string.Empty : dr["Descripcion"].ToString();
                            objExamen.TiempoMaximo = dr["TiempoMaximo"] is DBNull ? 0 : Convert.ToInt32(dr["TiempoMaximo"]);
                            objExamen.Clave = dr["Clave"] is DBNull ? string.Empty : dr["Clave"].ToString();
                            objExamen.EscalaCalificacion = dr["EscalaCalificacion"] is DBNull ? 0 : Convert.ToInt32(dr["EscalaCalificacion"]);
                            objExamen.LstPreguntas = ListaPreguntasPorExamen(IdExamen);

                            objExamen.Usuario.IdUsuario = dr["IdUsuario"] is DBNull ? 0 : Convert.ToInt32(dr["IdUsuario"]);
                            objExamen.Usuario.NombreCompleto = dr["NombreCompleto"] is DBNull ? string.Empty : dr["NombreCompleto"].ToString();
                            objExamen.Usuario.Correo = dr["Correo"] is DBNull ? string.Empty : dr["Correo"].ToString();

                            objExamen.Asignatura.IdAsignatura = dr["IdAsignatura"] is DBNull ? 0 : Convert.ToInt32(dr["IdAsignatura"]);
                            objExamen.Asignatura.Nombre = dr["Nombre"] is DBNull ? string.Empty : Convert.ToString(dr["Nombre"]);
                        }
                        con.Close();
                    }
                }
                return objExamen;
            }
        }

        public ExamenRealizadoBE ObtenerExamenResolver(int IdExamen, int idExamenRealizado)
        {
            ExamenRealizadoBE examenRealizado = null;

            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ObtenerExamenResolver", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdExamen", IdExamen);
                    cmd.Parameters.AddWithValue("@IdExamenRealizado", idExamenRealizado);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            examenRealizado = new ExamenRealizadoBE();
                            examenRealizado.Examen = new ExamenBE();
                            examenRealizado.Usuario = new UsuarioBE();

                            examenRealizado.Examen.IdExamen = dr["IdExamen"] is DBNull ? 0 : Convert.ToInt32(dr["IdExamen"]);
                            examenRealizado.Examen.FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]);
                            examenRealizado.Examen.FechaExpiracion = Convert.ToDateTime(dr["FechaExpiracion"]);
                            examenRealizado.Examen.FechaRegString = examenRealizado.Examen.FechaRegistro.ToShortDateString();
                            examenRealizado.Examen.FechaExpString = examenRealizado.Examen.FechaExpiracion.ToShortDateString();
                            examenRealizado.Examen.Titulo = dr["Titulo"] is DBNull ? string.Empty : dr["Titulo"].ToString();
                            examenRealizado.Examen.Descripcion = dr["Descripcion"] is DBNull ? string.Empty : dr["Descripcion"].ToString();
                            examenRealizado.Examen.TiempoMaximo = dr["TiempoMaximo"] is DBNull ? 0 : Convert.ToInt32(dr["TiempoMaximo"]);
                            examenRealizado.Examen.TiempoRestante = dr["TiempoRestante"] is DBNull ? 0 : Convert.ToInt32(dr["TiempoRestante"]);
                            examenRealizado.Examen.LstPreguntas = ListaPreguntasPorExamen(IdExamen);
                            examenRealizado.Estado = dr["EstadoExamen"] is DBNull ? false : true;
                            examenRealizado.ValidaFechaExpiracion = dr["ValidaFechaExpiracion"] is DBNull ? 0 : Convert.ToInt32(dr["ValidaFechaExpiracion"]);
                            examenRealizado.Usuario.IdUsuario = dr["IdUsuario"] is DBNull ? 0 : Convert.ToInt32(dr["IdUsuario"]);
                            examenRealizado.Usuario.NombreCompleto = dr["NombreCompleto"] is DBNull ? string.Empty : dr["NombreCompleto"].ToString();
                            examenRealizado.Usuario.Correo = dr["Correo"] is DBNull ? string.Empty : dr["Correo"].ToString();
                        }
                        con.Close();
                    }
                }
                return examenRealizado;
            }
        }

        public List<ExamenBE> Usp_ObtenerExamenesPublicos()
        {
            List<ExamenBE> lstExamen = new List<ExamenBE>();
            ExamenBE objExamen = null;
            UsuarioBE objUsuario = null;
            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ObtenerExamenesPublicos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objExamen = new ExamenBE();
                            objExamen.IdExamen = dr["IdExamen"] is DBNull ? 0 : Convert.ToInt32(dr["IdExamen"]);
                            objExamen.FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]);
                            objExamen.FechaExpiracion = Convert.ToDateTime(dr["FechaExpiracion"]);
                            objExamen.FechaRegString = objExamen.FechaRegistro.ToShortDateString();
                            objExamen.FechaExpString = objExamen.FechaExpiracion.ToShortDateString();
                            objExamen.Titulo = dr["Titulo"] is DBNull ? string.Empty : dr["Titulo"].ToString();
                            objExamen.Descripcion = dr["Descripcion"] is DBNull ? string.Empty : dr["Descripcion"].ToString();
                            objExamen.TiempoMaximo = dr["TiempoMaximo"] is DBNull ? 0 : Convert.ToInt32(dr["TiempoMaximo"]);
                            objExamen.NroPreguntas = dr["NroPreguntas"] is DBNull ? 0 : Convert.ToInt32(dr["NroPreguntas"]);

                            objUsuario = new UsuarioBE();
                            objUsuario.IdUsuario = dr["IdUsuario"] is DBNull ? 0 : Convert.ToInt32(dr["IdUsuario"]);
                            objUsuario.Nombres = dr["Nombres"] is DBNull ? string.Empty : dr["Nombres"].ToString();
                            objUsuario.ApellidoPaterno = dr["ApellidoPaterno"] is DBNull ? string.Empty : dr["ApellidoPaterno"].ToString();
                            objUsuario.ApellidoMaterno = dr["ApellidoMaterno"] is DBNull ? string.Empty : dr["ApellidoMaterno"].ToString();
                            objUsuario.imgData = dr["imagen"] is DBNull ? string.Empty : dr["imagen"].ToString();

                            objExamen.Usuario = objUsuario;
                            lstExamen.Add(objExamen);
                        }
                        con.Close();
                    }
                }
                return lstExamen;
            }
        }

        public List<PreguntaBE> ListaPreguntasPorExamen(int IdExamen)
        {
            List<PreguntaBE> Lista = new List<PreguntaBE>();
            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListarPreguntasPorExamen", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdExamen", IdExamen);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            PreguntaBE obj = new PreguntaBE();
                            obj.IdPregunta = dr["IdPregunta"] is DBNull ? 0 : Convert.ToInt32(dr["IdPregunta"]);
                            obj.Pregunta = dr["Pregunta"] is DBNull ? string.Empty : dr["Pregunta"].ToString();
                            obj.Numero = dr["Numero"] is DBNull ? 0 : Convert.ToInt32(dr["Numero"]);
                            //obj.Examen.IdExamen = dr["IdExamen"] is DBNull ? 0 : Convert.ToInt32(dr["IdExamen"]);
                            obj.LstAlternativa = ListaAlternativaPorPregunta(obj.IdPregunta);
                            Lista.Add(obj);
                        }
                        con.Close();
                    }
                }
                return Lista;
            }
        }

        public List<AlternativaBE> ListaAlternativaPorPregunta(int IdPregunta)
        {
            List<AlternativaBE> Lista = new List<AlternativaBE>();
            AlternativaBE objAlternativa = null;
            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListarAlternativasPorPregunta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdPregunta", IdPregunta);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objAlternativa = new AlternativaBE();
                            objAlternativa.Pregunta = new PreguntaBE();

                            objAlternativa.IdAlternativa = dr["IdAlternativa"] is DBNull ? 0 : Convert.ToInt32(dr["IdAlternativa"]);
                            objAlternativa.Pregunta.IdPregunta = dr["IdPregunta"] is DBNull ? 0 : Convert.ToInt32(dr["IdPregunta"]);
                            objAlternativa.Numero = dr["Numero"] is DBNull ? 0 : Convert.ToInt32(dr["Numero"]);
                            objAlternativa.Descripcion = dr["Descripcion"] is DBNull ? string.Empty : Convert.ToString(dr["Descripcion"]);
                            objAlternativa.OpcionCorrecta = dr["OpcionCorrecta"] is DBNull ? false : Convert.ToBoolean(dr["OpcionCorrecta"]);
                            objAlternativa.CantAltCorrectas = dr["CantAltCorrectas"] is DBNull ? 0 : Convert.ToInt32(dr["CantAltCorrectas"]);
                            Lista.Add(objAlternativa);
                        }

                        con.Close();
                    }
                }
                return Lista;
            }
        }

        public ExamenRealizadoBE ObtenerExamenRealizado(int idExamenRealizado)
        {
            ExamenRealizadoBE examenRealizado = null;

            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ObtenerExamenRealizado", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdExamenRealizado", idExamenRealizado);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            examenRealizado = new ExamenRealizadoBE();
                            examenRealizado.Usuario = new UsuarioBE();
                            examenRealizado.Examen = new ExamenBE();

                            examenRealizado.IdExamenRealizado = dr["IdExamenRealizado"] is DBNull ? 0 : Convert.ToInt32(dr["IdExamenRealizado"]);
                            examenRealizado.Usuario.IdUsuario = dr["IdUsuario"] is DBNull ? 0 : Convert.ToInt32(dr["IdUsuario"]); ;
                            examenRealizado.Examen.IdExamen = dr["IdExamen"] is DBNull ? 0 : Convert.ToInt32(dr["IdExamen"]);
                            examenRealizado.Examen = ObtenerExamen(examenRealizado.Examen.IdExamen);
                            examenRealizado.FechaRealizacion = Convert.ToDateTime(dr["FechaRealizacion"]);
                            examenRealizado.StrFechaRealizacion = examenRealizado.FechaRealizacion.ToShortDateString();
                            examenRealizado.FechaTermino = Convert.ToDateTime(dr["FechaTermino"]);
                            examenRealizado.StrFechaTermino = examenRealizado.FechaTermino.ToShortDateString();
                            examenRealizado.TotalPreguntas = dr["TotalPreguntas"] is DBNull ? 0 : Convert.ToInt32(dr["TotalPreguntas"]);
                            examenRealizado.NumeroPreguntasCorrectas = dr["NumeroPreguntasCorrectas"] is DBNull ? 0 : Convert.ToInt32(dr["NumeroPreguntasCorrectas"]);
                            examenRealizado.Estado = dr["Estado"] is DBNull ? false : Convert.ToBoolean(dr["Estado"]);
                            examenRealizado.LstExamenRealizadoDetalle = ObtenerExamenRealizadoDetallePorExamen(examenRealizado.IdExamenRealizado);
                        }
                    }
                }
            }

            return examenRealizado;
        }

        public List<PreguntaBE> ListarPreguntasConRespuestaPorExamen(int IdExamen)
        {
            List<PreguntaBE> Lista = new List<PreguntaBE>();
            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListarPreguntasPorExamen", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdExamen", IdExamen);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            PreguntaBE obj = new PreguntaBE();
                            obj.IdPregunta = dr["IdPregunta"] is DBNull ? 0 : Convert.ToInt32(dr["IdPregunta"]);
                            obj.Pregunta = dr["Pregunta"] is DBNull ? string.Empty : dr["Pregunta"].ToString();
                            obj.Numero = dr["Numero"] is DBNull ? 0 : Convert.ToInt32(dr["Numero"]);
                            obj.LstAlternativa = ListaAlternativaPorPregunta(obj.IdPregunta);
                            obj.LstAlternativaCorrecta = ListarAlternativasCorrectasPorPregunta(obj.IdPregunta);
                            Lista.Add(obj);
                        }
                        con.Close();
                    }
                }
                return Lista;
            }
        }

        public List<AlternativaBE> ListarAlternativasCorrectasPorPregunta(int IdPregunta)
        {
            List<AlternativaBE> Lista = new List<AlternativaBE>();
            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListarAlternativasPorPregunta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdPregunta", IdPregunta);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            AlternativaBE obj = new AlternativaBE();
                            obj.IdAlternativa = dr["IdAlternativa"] is DBNull ? 0 : Convert.ToInt32(dr["IdAlternativa"]);
                            obj.Numero = dr["Numero"] is DBNull ? 0 : Convert.ToInt32(dr["Numero"]);
                            obj.Descripcion = dr["Descripcion"] is DBNull ? string.Empty : Convert.ToString(dr["Descripcion"]);
                            obj.OpcionCorrecta = dr["OpcionCorrecta"] is DBNull ? false : Convert.ToBoolean(dr["OpcionCorrecta"]);
                            Lista.Add(obj);
                        }

                        con.Close();
                    }
                }
                return Lista;
            }
        }

        public List<AlternativaBE> ListaAlternativaCorrectasPorExamen(int IdExamen) {

            List<AlternativaBE> Lista = new List<AlternativaBE>();
            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListarPreguntasCorrectas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdExamen", IdExamen);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            AlternativaBE obj = new AlternativaBE();
                            obj.Numero = dr["Numero"] is DBNull ? 0 : Convert.ToInt32(dr["Numero"]);
                            obj.Descripcion = dr["Descripcion"] is DBNull ? string.Empty : Convert.ToString(dr["Descripcion"]);
                            Lista.Add(obj);
                        }

                        con.Close();
                    }
                }
                return Lista;
            }

        }

        public RespuestaBE RegistrarExamenRealizadoTemp(int IdUsuario, int IdExamen) {
            RespuestaBE respuesta = new RespuestaBE();
            int idExamenRealizado = -1;
            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd1 = new SqlCommand("Usp_RegistrarExamenRealizado", con))
                {
                    try
                    {
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                        cmd1.Parameters.AddWithValue("@IdExamen", IdExamen);
                        idExamenRealizado = Convert.ToInt32(cmd1.ExecuteScalar());
                        cmd1.Dispose();
                        respuesta.Registra = true;
                    }   
                    catch (Exception ex)
                    {
                        idExamenRealizado = -1;
                        respuesta.MensajeError = "Ocurrió un error al registrar. Contacte con el administrador.";
                        respuesta.MensajeException = ex.Message;
                        respuesta.Registra = false;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            respuesta.IdExamenRealizado = idExamenRealizado;
            return respuesta;
        }

        public List<ExamenRealizadoBE> ObtenerExamenesPendientesPorUsuario(int idUsuario)
        {
            List<ExamenRealizadoBE> lstExamenPendiente = new List<ExamenRealizadoBE>();
            ExamenRealizadoBE examenRealizado = null;

            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ObtenerExamenesPendientesPorUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            examenRealizado = new ExamenRealizadoBE();
                            examenRealizado.Examen = new ExamenBE();
                            examenRealizado.Usuario = new UsuarioBE();

                            examenRealizado.IdExamenRealizado = dr["IdExamenRealizado"] is DBNull ? 0 : Convert.ToInt32(dr["IdExamenRealizado"]);
                            examenRealizado.Examen.IdExamen = dr["IdExamen"] is DBNull ? 0 : Convert.ToInt32(dr["IdExamen"]);
                            examenRealizado.Examen.FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]);
                            examenRealizado.Examen.FechaExpiracion = Convert.ToDateTime(dr["FechaExpiracion"]);
                            examenRealizado.Examen.FechaRegString = examenRealizado.Examen.FechaRegistro.ToShortDateString();
                            examenRealizado.Examen.FechaExpString = examenRealizado.Examen.FechaExpiracion.ToShortDateString();
                            examenRealizado.Examen.Titulo = dr["Titulo"] is DBNull ? string.Empty : dr["Titulo"].ToString();
                            examenRealizado.Examen.Descripcion = dr["Descripcion"] is DBNull ? string.Empty : dr["Descripcion"].ToString();
                            examenRealizado.Examen.TiempoMaximo = dr["TiempoMaximo"] is DBNull ? 0 : Convert.ToInt32(dr["TiempoMaximo"]);
                            examenRealizado.Examen.TiempoRestante = dr["TiempoRestante"] is DBNull ? 0 : Convert.ToInt32(dr["TiempoRestante"]);
                            examenRealizado.Examen.LstPreguntas = ListaPreguntasPorExamen(examenRealizado.Examen.IdExamen);
                            examenRealizado.Estado = dr["EstadoExamen"] is DBNull ? false : true;
                            examenRealizado.FechaRealizacion = Convert.ToDateTime(dr["FechaRealizacion"]);
                            examenRealizado.StrFechaRealizacion = examenRealizado.FechaRealizacion.ToShortDateString() + " - " + examenRealizado.FechaRealizacion.ToShortTimeString();
                            examenRealizado.Usuario.IdUsuario = dr["IdUsuario"] is DBNull ? 0 : Convert.ToInt32(dr["IdUsuario"]);
                            examenRealizado.Usuario.NombreCompleto = dr["NombreCompleto"] is DBNull ? string.Empty : dr["NombreCompleto"].ToString();
                            examenRealizado.Usuario.Correo = dr["Correo"] is DBNull ? string.Empty : dr["Correo"].ToString();

                            lstExamenPendiente.Add(examenRealizado);
                        }
                        con.Close();
                    }
                }
            }

            return lstExamenPendiente;
        }

        public List<ExamenRealizadoBE> ObtenerExamenesRealizadosPorUsuario(int idUsuario)
        {
            List<ExamenRealizadoBE> lstExamenRealizado = new List<ExamenRealizadoBE>();
            ExamenRealizadoBE examenRealizado = null;

            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ObtenerExamenesRealizadosPorUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdUsuario", idUsuario);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            examenRealizado = new ExamenRealizadoBE();
                            examenRealizado.Usuario = new UsuarioBE();
                            examenRealizado.Examen = new ExamenBE();
                            
                            examenRealizado.IdExamenRealizado = dr["IdExamenRealizado"] is DBNull ? 0 : Convert.ToInt32(dr["IdExamenRealizado"]);
                            examenRealizado.Usuario.IdUsuario = idUsuario;
                            examenRealizado.Examen.IdExamen = dr["IdExamen"] is DBNull ? 0 : Convert.ToInt32(dr["IdExamen"]);
                            examenRealizado.Examen = ObtenerExamen(examenRealizado.Examen.IdExamen);
                            examenRealizado.FechaRealizacion = Convert.ToDateTime(dr["FechaRealizacion"]);
                            examenRealizado.StrFechaRealizacion = examenRealizado.FechaRealizacion.ToShortDateString();
                            examenRealizado.FechaTermino = Convert.ToDateTime(dr["FechaTermino"]);
                            examenRealizado.StrFechaTermino = examenRealizado.FechaTermino.ToShortDateString();
                            examenRealizado.TotalPreguntas = dr["TotalPreguntas"] is DBNull ? 0 : Convert.ToInt32(dr["TotalPreguntas"]);
                            examenRealizado.NumeroPreguntasCorrectas = dr["NumeroPreguntasCorrectas"] is DBNull ? 0 : Convert.ToInt32(dr["NumeroPreguntasCorrectas"]);
                            examenRealizado.Estado = dr["Estado"] is DBNull ? false : Convert.ToBoolean(dr["Estado"]);
                            examenRealizado.LstExamenRealizadoDetalle = ObtenerExamenRealizadoDetallePorExamen(examenRealizado.IdExamenRealizado);

                            lstExamenRealizado.Add(examenRealizado);
                        }
                    }
                }
            }

            return lstExamenRealizado;
        }

        public List<ExamenBE> ObtenerExamenesCreadosPorUsuario(int idUsuario)
        {
            List<ExamenBE> lstExamen = new List<ExamenBE>();
            ExamenBE objExamen = null;

            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ObtenerExamenesCreadosPorUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdUsuario", idUsuario);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objExamen = new ExamenBE();
                            objExamen.Usuario = new UsuarioBE();
                            
                            objExamen.IdExamen = dr["IdExamen"] is DBNull ? 0 : Convert.ToInt32(dr["IdExamen"]);
                            objExamen.FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]);
                            objExamen.FechaExpiracion = Convert.ToDateTime(dr["FechaExpiracion"]);
                            objExamen.FechaRegString = Convert.ToDateTime(dr["FechaRegistro"]).ToShortDateString();
                            objExamen.FechaExpString = Convert.ToDateTime(dr["FechaExpiracion"]).ToShortDateString();
                            objExamen.Titulo = dr["Titulo"] is DBNull ? string.Empty : dr["Titulo"].ToString();
                            objExamen.Descripcion = dr["Descripcion"] is DBNull ? string.Empty : dr["Descripcion"].ToString();
                            objExamen.TiempoMaximo = dr["TiempoMaximo"] is DBNull ? 0 : Convert.ToInt32(dr["TiempoMaximo"]);
                            objExamen.Clave = dr["Clave"] is DBNull ? string.Empty : Convert.ToString(dr["Clave"]);
                            objExamen.EscalaCalificacion = dr["EscalaCalificacion"] is DBNull ? 0 : Convert.ToInt32(dr["EscalaCalificacion"]);
                            objExamen.LstPreguntas = ListaPreguntasPorExamen(objExamen.IdExamen);
                            objExamen.Usuario.IdUsuario = dr["IdUsuario"] is DBNull ? 0 : Convert.ToInt32(dr["IdUsuario"]);
                            lstExamen.Add(objExamen);
                        }
                    }
                }
            }

            return lstExamen;
        }

        public List<ExamenRealizadoDetalleBE> ObtenerExamenRealizadoDetallePorExamen(int idExamenRealizado)
        {
            List<ExamenRealizadoDetalleBE> lstExamenRealizadoDetalle = new List<ExamenRealizadoDetalleBE>();
            ExamenRealizadoDetalleBE detalle = null;

            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ObtenerExamenRealizadoDetallePorExamen", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdExamenRealizado", idExamenRealizado);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            detalle = new ExamenRealizadoDetalleBE();
                            detalle.IdExamenRealizadoDetalle = dr["IdExamenRealizadoDetalle"] is DBNull ? 0 : Convert.ToInt32(dr["IdExamenRealizadoDetalle"]);
                            detalle.IdExamenRealizado = dr["IdExamenRealizado"] is DBNull ? 0 : Convert.ToInt32(dr["IdExamenRealizado"]);
                            detalle.IdPregunta = dr["IdPregunta"] is DBNull ? 0 : Convert.ToInt32(dr["IdPregunta"]);
                            detalle.IdAlternativa = dr["IdAlternativa"] is DBNull ? 0 : Convert.ToInt32(dr["IdAlternativa"]);

                            lstExamenRealizadoDetalle.Add(detalle);
                        }
                    }
                }
            }

            return lstExamenRealizadoDetalle;
        }
            
        public string RegistrarExamenRealizado(ExamenRealizadoBE objExamenRealizado, List<AlternativaBE> objAltMarcUsua)
        {
            string registra = "ERROR";

            var ArrayAltCorrectas = ListaAlternativaCorrectasPorExamen(objExamenRealizado.Examen.IdExamen);
            var CountAltCorrectas = 0;

            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand cmd1 = new SqlCommand("Usp_ActualizarExamenRealizado", con, tr))
                {
                    try
                    {
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@IdUsuario", objExamenRealizado.Usuario.IdUsuario);
                        cmd1.Parameters.AddWithValue("@IdExamen", objExamenRealizado.Examen.IdExamen);
                        cmd1.Parameters.AddWithValue("@TotalPreguntas", objExamenRealizado.TotalPreguntas);

                        for (int i = 0; i < objExamenRealizado.TotalPreguntas; i++) {
                            if (objAltMarcUsua[i].Descripcion == ArrayAltCorrectas[i].Descripcion) {
                                CountAltCorrectas = CountAltCorrectas + 1;
                            }
                        }

                        cmd1.Parameters.AddWithValue("@NumeroPreguntasCorrectas", CountAltCorrectas);
                        int idExamenRealizado = Convert.ToInt32(cmd1.ExecuteScalar());

                        // Se registra las preguntas del examen

                        if (objExamenRealizado.LstExamenRealizadoDetalle != null) {
                            foreach (var det in objExamenRealizado.LstExamenRealizadoDetalle)
                            {
                                SqlCommand cmd2 = new SqlCommand("Usp_InsertarExamenRealizadoDetalle", con, tr);
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@IdExamenRealizado", idExamenRealizado);
                                cmd2.Parameters.AddWithValue("@IdPregunta", det.IdPregunta);
                                cmd2.Parameters.AddWithValue("@IdAlternativa", det.IdAlternativa);
                                cmd2.ExecuteNonQuery();
                            }
                        }
                        

                        tr.Commit();
                        registra = "OK" + "," + idExamenRealizado;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        registra = "ERROR";
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return registra;
        }

        public bool CerrarExamenesExpiradosPorUsuario(int idUsuario)
        {
            bool cerrado = false;
            int filas = 0;
            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_CerrarExamenesExpiradosPorUsuario", con))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        filas = cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        cerrado = true;
                    }
                    catch (Exception ex)
                    {
                        cerrado = false;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return cerrado;
        }
    }
}
