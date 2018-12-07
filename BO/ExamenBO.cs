using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAO;

namespace BO
{
    public class ExamenBO
    {
        public RespuestaBE RegistrarExamen(ExamenBE examen)
        {
            ExamenDAO dao = new ExamenDAO();
            return dao.RegistrarExamen(examen);
        }

        public ExamenBE ObtenerExamen(int IdExamen)
        {
            ExamenDAO dao = new ExamenDAO();
            return dao.ObtenerExamen(IdExamen);
        }

        public ExamenRealizadoBE ObtenerExamenResolver(int IdExamen, int idExamenRealizado)
        {
            ExamenDAO dao = new ExamenDAO();
            return dao.ObtenerExamenResolver(IdExamen, idExamenRealizado);
        }

        public List<ExamenBE> Usp_ObtenerExamenesPublicos()
        {
            ExamenDAO dao = new ExamenDAO();
            return dao.Usp_ObtenerExamenesPublicos();
        }

        public List<AlternativaBE> ListaAlternativaCorrectasPorExamen(int IdExamen)
        {
            ExamenDAO dao = new ExamenDAO();
            return dao.ListaAlternativaCorrectasPorExamen(IdExamen);
        }

        public RespuestaBE RegistrarExamenRealizadoTemp(int IdUsuario, int IdExamen)
        {
            ExamenDAO dao = new ExamenDAO();
            return dao.RegistrarExamenRealizadoTemp(IdUsuario, IdExamen);
        }

        public string RegistrarExamenRealizado(ExamenRealizadoBE objExamenRealizado, List<AlternativaBE> objAltMarcUsua)
        {
            ExamenDAO dao = new ExamenDAO();
            return dao.RegistrarExamenRealizado(objExamenRealizado, objAltMarcUsua);
        }


        public ExamenRealizadoBE ObtenerExamenRealizado(int idExamenRealizado)
        {
            ExamenDAO dao = new ExamenDAO();
            return dao.ObtenerExamenRealizado(idExamenRealizado);
        }

        public List<ExamenRealizadoBE> ObtenerExamenesRealizadosPorUsuario(int idUsuario)
        {
            ExamenDAO dao = new ExamenDAO();
            return dao.ObtenerExamenesRealizadosPorUsuario(idUsuario);
        }

        public List<ExamenBE> ObtenerExamenesCreadosPorUsuario(int idUsuario)
        {
            ExamenDAO dao = new ExamenDAO();
            return dao.ObtenerExamenesCreadosPorUsuario(idUsuario);
        }
        

        public List<ExamenRealizadoBE> ObtenerExamenesPendientesPorUsuario(int idUsuario)
        {
            ExamenDAO dao = new ExamenDAO();
            return dao.ObtenerExamenesPendientesPorUsuario(idUsuario);
        }
        
        public Boolean CerrarExamenesExpiradosPorUsuario(int idUsuario)
        {
            ExamenDAO dao = new ExamenDAO();
            return dao.CerrarExamenesExpiradosPorUsuario(idUsuario);
        }
    }
}
