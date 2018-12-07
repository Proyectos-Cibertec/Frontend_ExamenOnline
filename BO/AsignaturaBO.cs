using BE;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class AsignaturaBO
    {

        public List<AsignaturaBE> ObtenerAsignatura() {
            AsignaturoDAO dao = new AsignaturoDAO();
            return dao.ObtenerAsignatura();
        }

    }
}
