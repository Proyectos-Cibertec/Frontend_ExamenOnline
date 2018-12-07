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
    public class AsignaturoDAO
    {
        Conexion conexion = new Conexion();

        public List<AsignaturaBE> ObtenerAsignatura()
        {
            List<AsignaturaBE> lstAsignatura = new List<AsignaturaBE>();
            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("USP_LISTA_ASIGNATURAS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            AsignaturaBE obj = new AsignaturaBE();
                            obj.IdAsignatura = dr["IdAsignatura"] is DBNull ? 0 : Convert.ToInt32(dr["IdAsignatura"]);
                            obj.Nombre = Convert.ToString(dr["Nombre"]);
                            obj.Estado = Convert.ToBoolean(dr["Estado"]);
                            lstAsignatura.Add(obj);
                        }
                        con.Close();
                    }
                }
                return lstAsignatura;
            }
        }

    }
}
