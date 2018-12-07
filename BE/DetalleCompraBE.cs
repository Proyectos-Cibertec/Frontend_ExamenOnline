using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class DetalleCompraBE
    {
        public int IdDetalleCompra { get; set; }
        public int IdCompra { get; set; }
        public int IdSuscripcion { get; set; }
        public int CantidadMeses { get; set; }
        public decimal PrecioPorMes { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Estado { get; set; } // 0: Ya expiró | 1: No expira
    }
}
