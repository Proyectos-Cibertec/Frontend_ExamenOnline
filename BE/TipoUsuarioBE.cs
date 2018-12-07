using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class TipoUsuarioBE
    {
        public int IdTipoUsuario { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public String Crea { get; set; }
        public String Modifica { get; set; }
        public String Elimina { get; set; }
    }
}
