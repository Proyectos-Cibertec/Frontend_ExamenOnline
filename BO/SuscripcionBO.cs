using BE;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class SuscripcionBO
    {
        public List<SuscripcionBE> lstSuscripciones() {
            SuscripcionDAO dao = new SuscripcionDAO();
            return dao.lstSuscripciones();
        }
    }
}
