using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class ExamenBE
    {
        public int IdExamen { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string FechaRegString { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public string FechaExpString { get; set; }
        public UsuarioBE Usuario { get; set; }
        public TipoExamenBE TipoExamen { get; set; }
        public AsignaturaBE Asignatura { get; set; }
        public string Titulo { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public int TiempoMaximo { get; set; }
        public int TiempoRestante { get; set; }
        public int EscalaCalificacion { get; set; }
        public int NroPreguntas { get; set; }
        public List<PreguntaBE> LstPreguntas { get; set; }
    }
}
