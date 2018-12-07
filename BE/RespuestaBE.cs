using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class RespuestaBE
    {
        public string MensajeError { get; set; }
        public string MensajeException { get; set; }
        public int CodigoError { get; set; }
        public Boolean Registra { get; set; }
        public int IdExamenRealizado { get; set; }
    }
}
