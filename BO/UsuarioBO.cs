using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAO;

namespace BO
{
    public class UsuarioBO
    {
        public UsuarioBE ObtenerUsuario(string usuario)
        {
            UsuarioDAO dao = new UsuarioDAO();
            return dao.ObtenerUsuario(usuario);
        }

        public UsuarioBE IniciarSesion(UsuarioBE usuario)
        {
            UsuarioDAO dao = new UsuarioDAO();
            return dao.IniciarSesion(usuario);
        }

        public RespuestaBE RegistrarUsuario(UsuarioBE usuario)
        {
            UsuarioDAO dao = new UsuarioDAO();
            return dao.RegistrarUsuario(usuario);
        }   

    }
}
