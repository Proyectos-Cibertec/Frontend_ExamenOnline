using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class CategoriaBE
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public string IconoCategoria { get; set; }
        public string UrlCategoria { get; set; }
        public RolBE Rol { get; set; }
        public bool Estado { get; set; }
        public String Crea { get; set; }
        public String Modifica { get; set; }
        public String Elimina { get; set; }
    }
}
