using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class ExamenRealizadoBE
    {
        public int IdExamenRealizado { get; set; }
        public UsuarioBE Usuario { get; set; }
        public ExamenBE Examen { get; set; }
        public DateTime FechaRealizacion { get; set; }
        public string StrFechaRealizacion { get; set; }
        public DateTime FechaTermino { get; set; }
        public string StrFechaTermino { get; set; }
        public int TotalPreguntas { get; set; }
        public int NumeroPreguntasCorrectas { get; set; }
        public bool Estado { get; set; }
        public List<ExamenRealizadoDetalleBE> LstExamenRealizadoDetalle { get; set; }

        public int ValidaFechaExpiracion { get; set; }
    }
}
