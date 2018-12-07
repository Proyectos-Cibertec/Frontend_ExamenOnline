using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class SuscripcionBE
    {
        public int IdSuscripcion { get; set; }
        public string Descripcion { get; set; }
        public double precio { get; set; }
        public int idTipoUsuario { get; set; }
        public TipoUsuarioBE TipoUsuario { get; set; }
        public bool Estado { get; set; }
        public String Crea { get; set; }
        public String Modifica { get; set; }
        public String Elimina { get; set; }
    }
}
