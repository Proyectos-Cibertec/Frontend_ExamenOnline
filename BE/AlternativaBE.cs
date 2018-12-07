using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class AlternativaBE
    {
        public int IdAlternativa { get; set; }
        public PreguntaBE Pregunta { get; set; }
        public int Numero { get; set; }
        public string Descripcion { get; set; }
        public bool OpcionCorrecta { get; set; }
        public int CantAltCorrectas { get; set; }
    }
}
