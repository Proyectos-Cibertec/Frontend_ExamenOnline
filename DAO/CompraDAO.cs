using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class CompraDAO
    {
        Conexion conexion = new Conexion();

        public bool RegistrarCompra(CompraBE compra)
        {
            bool respuesta = true;

            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand cmd1 = new SqlCommand("USP_REGISTRA_COMPRA", con, tr))
                {
                    try
                    {
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@idUsuario", compra.IdUsuario);
                        cmd1.Parameters.AddWithValue("@FechaCompra", DateTime.Now);
                        cmd1.Parameters.AddWithValue("@Total", compra.Total);

                        // Se obtiene el Id generado del examen
                        int idCompra = Convert.ToInt32(cmd1.ExecuteScalar());

                        // Se registra las preguntas del examen
                        foreach (var item in compra.lstDetalleCompra)
                        {
                            SqlCommand cmd2 = new SqlCommand("USP_REGISTRA_DET_COMPRA", con, tr);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@idCompra", idCompra);
                            cmd2.Parameters.AddWithValue("@IdSuscripcion", item.IdSuscripcion);
                            cmd2.Parameters.AddWithValue("@CantidadMeses", item.CantidadMeses);
                            cmd2.Parameters.AddWithValue("@PrecioPorMes", item.PrecioPorMes);
                            cmd2.Parameters.AddWithValue("@Estado", 1);
                            cmd2.ExecuteNonQuery();

                            SqlCommand cmd3 = new SqlCommand("USP_ACTUALIZA_TIPO_USUARIO", con, tr);
                            cmd3.CommandType = CommandType.StoredProcedure;
                            cmd3.Parameters.AddWithValue("@idUsuario", compra.IdUsuario);
                            cmd3.Parameters.AddWithValue("@IdSuscripcion", item.IdSuscripcion);
                            cmd3.ExecuteNonQuery();

                        }
                        
                        tr.Commit();
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        respuesta = false;
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
