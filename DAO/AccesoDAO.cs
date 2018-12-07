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
    public class AccesoDAO
    {
        private Conexion conexion = new Conexion();

        public List<CategoriaBE> ListaCategoriaPorRol(string idRol)
        {
            List<CategoriaBE> lstCategoria = new List<CategoriaBE>();
            CategoriaBE oCategoria = null;
            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListarCategoriasPorRol", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdRol", idRol);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oCategoria = new CategoriaBE();
                            oCategoria.Rol = new RolBE();

                            oCategoria.IdCategoria = dr.GetInt32(0);
                            oCategoria.NombreCategoria = dr.GetString(1);
                            oCategoria.IconoCategoria = dr.GetString(2);
                            oCategoria.UrlCategoria = dr.IsDBNull(3) ? string.Empty : dr.GetString(3);
                            oCategoria.Rol.IdRol = dr.GetInt32(4);
                            oCategoria.Rol.NombreRol = dr.GetString(5);
                            lstCategoria.Add(oCategoria);
                        }
                        con.Close();
                    }
                }
            }

            return lstCategoria;
        }

        public List<EnlaceBE> ListarEnlacePorCategoria(int idCategoria)
        {
            List<EnlaceBE> lstEnlace = new List<EnlaceBE>();
            EnlaceBE oEnlace = null;
            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListarEnlacesPorCategoria", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oEnlace = new EnlaceBE();
                            oEnlace.IdEnlace = dr.GetInt32(0);
                            oEnlace.NombreEnlace = dr.GetString(1);
                            oEnlace.UrlEnlace = dr.GetString(2);
                            oEnlace.EnlaceIcono = dr.GetString(3);
                            lstEnlace.Add(oEnlace);
                        }
                        con.Close();
                    }
                }
            }

            return lstEnlace;
        }
    }
}
