using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BE
{
    public class UsuarioBE
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }
        public string Dni { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public string Contrasenia { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Intentos { get; set; }
        public bool Bloqueado { get; set; }
        public TipoUsuarioBE TipoUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public bool Estado { get; set; }
        public String Crea { get; set; }
        public String Modifica { get; set; }
        public String Elimina { get; set; }
        public HttpPostedFileBase imgUsuario { get; set; }
        public string imgData { get; set; }

        //Para categoria
        public RolBE Rol;

    }
}
