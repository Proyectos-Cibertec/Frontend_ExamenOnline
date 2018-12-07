using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Integrador.Controllers
{
    public class PresentacionController : Controller
    {
        // GET: Presentacion
        public ActionResult Index()
        {
            return View();
        }
    }
}