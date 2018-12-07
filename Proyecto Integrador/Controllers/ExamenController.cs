using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BE;
using BO;
using Servicio;
using System.IO;
using System.Data.OleDb;
using System.Data.Common;
using System.Web.Configuration;

namespace Proyecto_Integrador.Controllers
{
    public class ExamenController : Controller
    {
        // GET: Examen
        public ActionResult RegistrarExamenMenu()
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


        public ActionResult Registrar()   
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                UsuarioBE usuario = (UsuarioBE)Session["usuario"];
                int tipoUsuario = usuario.TipoUsuario.IdTipoUsuario;
                ViewBag.TipoUsuario = tipoUsuario;
                ViewBag.IdUsuario = usuario.IdUsuario;
                return View();
            }
        }

        public ActionResult Subir()
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

        public ActionResult ListarExamenes()
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
        
        [HttpPost]
        public ActionResult Subir(HttpPostedFileBase archivoExcel)
        {
            string ruta = WebConfigurationManager.AppSettings["RutaArchivosTemporales"];

            if (archivoExcel != null && archivoExcel.ContentLength > 0)
            {
                var fileName = Path.GetFileName(archivoExcel.FileName);
                DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/" + ruta));
                if (!d.Exists)
                {
                    Directory.CreateDirectory(Server.MapPath("~/" + ruta));
                }

                var path = Path.Combine(Server.MapPath("~/" + ruta + "/"), fileName);
                archivoExcel.SaveAs(path);

                // Transformar a json
                ViewBag.archivoSubido = "subido";
                ViewBag.rutaArchivoSubido = "/" + ruta + "/" + fileName;
                return View();
            }
            else
            {
                ViewBag.archivoSubido = "error";
                return View();
            }
        }

        public JsonResult EliminarArchivoTemporal(string rutaArchivo)
        {
            bool elimina = false;
            rutaArchivo = "~" + rutaArchivo;
            try
            {
                if (System.IO.File.Exists(Server.MapPath(rutaArchivo)))
                {
                    System.IO.File.Delete(Server.MapPath(rutaArchivo));
                    elimina = true;
                }
            }
            catch (Exception ex) {
                elimina = false;
            }

            return Json(elimina, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegistrarExamen(ExamenBE examen)
        {
            ExamenBO bo = new ExamenBO();
            RespuestaBE respuesta = new RespuestaBE();
            respuesta.Registra = false;
            examen.Clave = examen.Clave == null ? string.Empty : examen.Clave; // Por que viene null?

            // Debe estar logeado para registrar examen
            UsuarioBE usuario = (UsuarioBE)Session["usuario"];
            try
            {
                if (usuario != null)
                {
                    examen.Usuario = usuario;
                    if (usuario.TipoUsuario.IdTipoUsuario == 0 && examen.Clave.Length > 0)
                    {
                        respuesta.Registra = false;
                        respuesta.MensajeError = "El usuario de tipo FREE no puede ponerle clave a su examen. Solo se le permite al usuario QUE HAYA PAGADO UNA SUSCRIPCIÓN";
                        respuesta.CodigoError = -1;
                    }
                    else
                    {
                        respuesta = bo.RegistrarExamen(examen);
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.MensajeError = ex.Message;
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Usp_ObtenerExamenesPublicos()
        {
            ExamenBO bo = new ExamenBO();
            List<ExamenBE> lstExamen = bo.Usp_ObtenerExamenesPublicos();
            return Json(lstExamen, JsonRequestBehavior.AllowGet);
        }

        #region Metodos Para Resolver Examen

        public ActionResult ResolverExamenConClave()
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

        public ActionResult ResolverExamen(int IdExamen, int idExamenRealizado)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                UsuarioBE usuario = (UsuarioBE)Session["usuario"];
                ViewBag.idUsuario = usuario.IdUsuario;
                ViewBag.imgData = usuario.imgData;
                ViewBag.idExamenRealizado = idExamenRealizado;
                ViewBag.IdExamen = IdExamen;

                return View();
            }
        }

        public JsonResult ObtenerExamenResolver(int IdExamen, int idExamenRealizado)
        {
            ExamenBO bo = new ExamenBO();
            UsuarioBE usuario = (UsuarioBE)Session["usuario"];
            ExamenRealizadoBE examenRealizado = null;

            if (usuario != null)
            {
                examenRealizado = bo.ObtenerExamenResolver(IdExamen, idExamenRealizado);
            }
            
            return Json(examenRealizado, JsonRequestBehavior.AllowGet);
        }

        /*public JsonResult RegistrarExamenRealizadoTemp(int IdExamen)
        {
            RespuestaBE respuesta = new RespuestaBE();
            respuesta.IdExamenRealizado = -1;
            ExamenBO bo = new ExamenBO();

            // Valida que el usuario se encuentre logeado
            UsuarioBE usuario = (UsuarioBE)Session["usuario"];
            if (usuario != null)
            {
                respuesta = bo.RegistrarExamenRealizadoTemp(usuario.IdUsuario, IdExamen);
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }*/

        public JsonResult RegistrarExamenRealizadoTemp(int IdExamen, string clave)
        {
            RespuestaBE respuesta = new RespuestaBE();
            respuesta.IdExamenRealizado = -1;
            respuesta.Registra = false;
            
            ExamenBO bo = new ExamenBO();

            // Valida que el usuario se encuentre logeado
            UsuarioBE usuario = (UsuarioBE)Session["usuario"];
            if (usuario != null)
            {
                ExamenBE examen = bo.ObtenerExamen(IdExamen);
                if (examen != null)
                {
                    if (examen.Clave == clave)
                    {
                        respuesta = bo.RegistrarExamenRealizadoTemp(usuario.IdUsuario, IdExamen);
                    }
                    else
                    {
                        respuesta.MensajeError = "La clave del examen no es la correcta";
                    }
                }
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegistrarExamenRealizado(ExamenRealizadoBE objExamenRealizado, List<AlternativaBE> objAltMarcUsua)
        {
            string registra = "ERROR";
            ExamenBO bo = new ExamenBO();

            // Debe estar logeado para registrar examen
            UsuarioBE usuario = (UsuarioBE)Session["usuario"];
            if (usuario != null)
            {
                UsuarioBE us = new UsuarioBE();
                us.IdUsuario = usuario.IdUsuario;
                objExamenRealizado.Usuario = us;
                registra = bo.RegistrarExamenRealizado(objExamenRealizado, objAltMarcUsua);
            }

            return Json(registra, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CerrarExamenesExpiradosPorUsuario()
        {
            bool respuesta = false;
            ExamenBO bo = new ExamenBO();
            UsuarioBE usuario = (UsuarioBE)Session["usuario"];

            if (usuario != null)
            {
                respuesta = bo.CerrarExamenesExpiradosPorUsuario(usuario.IdUsuario);
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Metodos para visualizar examen corregido

        public ActionResult MisExamenesRealizados()
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

        public ActionResult MisExamenesCreados()
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

        public ActionResult MisExamenesPendientes()
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

        public JsonResult ObtenerExamenesRealizadosPorUsuario()
        {
            ExamenBO bo = new ExamenBO();
            List<ExamenRealizadoBE> lstExamenRealizado = new List<ExamenRealizadoBE>();

            if (Session["usuario"] != null)
            {
                UsuarioBE usuario = (UsuarioBE)Session["usuario"];
                lstExamenRealizado = bo.ObtenerExamenesRealizadosPorUsuario(usuario.IdUsuario);
            }

            return Json(lstExamenRealizado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerExamenesCreadosPorUsuario()
        {
            ExamenBO bo = new ExamenBO();
            List<ExamenBE> lstExamen = new List<ExamenBE>();

            if (Session["usuario"] != null)
            {
                UsuarioBE usuario = (UsuarioBE)Session["usuario"];
                int tipoUsuario = usuario.TipoUsuario.IdTipoUsuario;
                ViewBag.TipoUsuario = tipoUsuario;
                lstExamen = bo.ObtenerExamenesCreadosPorUsuario(usuario.IdUsuario);
            }

            return Json(lstExamen, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerExamenesPendientesPorUsuario()
        {
            ExamenBO bo = new ExamenBO();
            List<ExamenRealizadoBE> lstExamenRealizado = new List<ExamenRealizadoBE>();

            if (Session["usuario"] != null)
            {
                UsuarioBE usuario = (UsuarioBE)Session["usuario"];
                lstExamenRealizado = bo.ObtenerExamenesPendientesPorUsuario(usuario.IdUsuario);
            }

            return Json(lstExamenRealizado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExamenRealizado(int idExamenRealizado)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.idExamenRealizado = idExamenRealizado;
                ExamenBO bo = new ExamenBO();
                ExamenRealizadoBE examenRealizado = bo.ObtenerExamenRealizado(idExamenRealizado);
                return View(examenRealizado);
            }
        }

        public JsonResult ObtenerExamenRealizado(int idExamenRealizado)
        {
            ExamenBO bo = new ExamenBO();
            ExamenRealizadoBE examenRealizado = bo.ObtenerExamenRealizado(idExamenRealizado);
            return Json(examenRealizado, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}