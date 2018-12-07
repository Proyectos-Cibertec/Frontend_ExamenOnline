using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Integrador.Controllers
{
    public class InicioController : Controller
    {
        public ActionResult MenuPrincipal()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();
            }
        }

    }
}