using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DAO
{
    public class Conexion
    {
        public String cn { get; set; }

        public Conexion()
        {
            // Obtiene la cadena de conexión al instanciar la clase Conexion
            cn = ConfigurationManager.ConnectionStrings["cn_ExamenOnline"].ToString();
        }
    }
}
