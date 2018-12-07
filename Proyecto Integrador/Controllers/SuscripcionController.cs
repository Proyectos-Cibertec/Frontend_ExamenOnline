using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Integrador.Controllers
{
    public class SuscripcionController : Controller
    {
        // GET: Suscripcion
        public JsonResult lstSuscripciones()
        {
            SuscripcionBO bo = new SuscripcionBO();
            return Json(bo.lstSuscripciones(), JsonRequestBehavior.AllowGet);
        }
    }
}