using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class CompraBE
    {
        public int IdCompra { get; set; }
        public int IdUsuario { get; set; }
        public double Total { get; set; }
        public DateTime FechaCompra { get; set; }
        public List<DetalleCompraBE> lstDetalleCompra { get; set; }
    }
}
