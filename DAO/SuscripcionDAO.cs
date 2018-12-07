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
    public class SuscripcionDAO
    {
        Conexion conexion = new Conexion();

        public List<SuscripcionBE> lstSuscripciones() {

            List<SuscripcionBE> lista = new List<SuscripcionBE>();
            using (SqlConnection con = new SqlConnection(conexion.cn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("USP_LISTA_SUSCRIPCIONES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            SuscripcionBE obj = new SuscripcionBE();
                            obj.IdSuscripcion = Convert.ToInt32(dr["IdSuscripcion"]);
                            obj.Descripcion = Convert.ToString(dr["Descripcion"]);
                            obj.precio = Convert.ToDouble(dr["Precio"]);
                            obj.idTipoUsuario = Convert.ToInt32(dr["idTipoUsuario"]);
                            lista.Add(obj);
                        }
                        con.Close();
                    }
                }
            }

            return lista;
        }

    }
}
