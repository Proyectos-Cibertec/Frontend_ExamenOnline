using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class VideoBE
    {
        public int IdVideo { get; set; }
        public string Video { get; set; }
        public PreguntaBE Pregunta { get; set; }
    }
}
