using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BE
{
    public class ImagenBE
    {
        public int IdImagen { get; set; }
        public string Imagen { get; set; }
        public PreguntaBE Pregunta { get; set; }
    }
}
