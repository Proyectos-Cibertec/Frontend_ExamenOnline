using BE;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilitarios;

namespace Proyecto_Integrador.Controllers
{
    public class ComprarController : Controller
    {
        // GET: Comprar
        public ActionResult Index()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {

                UsuarioBE usuario = (UsuarioBE)Session["usuario"];
                ViewBag.nombre = usuario.Nombres + " " + usuario.ApellidoPaterno + " " + usuario.ApellidoMaterno;
                ViewBag.correo = usuario.Correo;
                ViewBag.codigoUsu = usuario.IdUsuario;
                return View();
            }
        }

        public JsonResult RegistrarCompra(CompraBE compra) {

            CompraBO bo = new CompraBO();
            bool respuesta = true;
            // Debe estar logeado para registrar
            UsuarioBE usuario = (UsuarioBE)Session["usuario"];
            try
            {
                if (usuario != null)
                {
                   respuesta = bo.RegistrarCompra(compra);
                    if (respuesta == true) {
                        EnvioCorreo.EnviarCorreo(usuario, "Registro de Usuario", "Se ha suscrito en la plataforma correctamente. Su usuario es: <b>" + usuario.Usuario + "</b> y su clave es: <b>" + usuario.Contrasenia + "</b>");
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);

        }

    }
}