using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Integrador.Controllers
{
    public class AsignaturasController : Controller
    {
        // GET: Asignaturas
        public JsonResult ObtenerAsignatura()
        {
            AsignaturaBO bo = new AsignaturaBO();
            return Json(bo.ObtenerAsignatura(), JsonRequestBehavior.AllowGet);
        }
        
    }
}