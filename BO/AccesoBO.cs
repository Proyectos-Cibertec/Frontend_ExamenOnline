using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAO;

namespace BO
{
    public class AccesoBO
    {
        public List<CategoriaBE> ListaCategoriaPorRol(string idrol)
        {
            AccesoDAO dao = new AccesoDAO();
            return dao.ListaCategoriaPorRol(idrol);
        }

        public List<EnlaceBE> ListarEnlacePorCategoria(int idcategoria)
        {
            AccesoDAO dao = new AccesoDAO();
            return dao.ListarEnlacePorCategoria(idcategoria);
        }
    }
}
