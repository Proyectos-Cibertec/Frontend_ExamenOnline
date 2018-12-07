using BE;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CompraBO
    {
        public bool RegistrarCompra(CompraBE compra) {

            CompraDAO dao = new CompraDAO();
            return dao.RegistrarCompra(compra);

        }
    }
}
