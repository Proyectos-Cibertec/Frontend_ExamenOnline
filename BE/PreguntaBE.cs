using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class PreguntaBE
    {
        public int IdPregunta { get; set; }
        public string Pregunta { get; set; }
        public int Numero { get; set; }
        public ExamenBE Examen { get; set; }

        public List<ImagenBE> LstImagen { get; set; }
        public List<VideoBE> LstVideo { get; set; }
        public List<AlternativaBE> LstAlternativa { get; set; }
        public List<AlternativaBE> LstAlternativaCorrecta { get; set; }
    }
}
