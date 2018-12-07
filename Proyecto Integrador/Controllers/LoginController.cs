using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BE;
using BO;
using Servicio;
using Utilitarios;

namespace Proyecto_Integrador.Controllers
{
    public class LoginController : Controller
    {

        public ActionResult Index()
        {
            if (Session["usuario"] == null)
            {
                ViewBag.usuarioNoExiste = false;
                ViewBag.usuarioIntentos = 0;
                ViewBag.usuarioBloqueado = false;
                ViewBag.usuarioError = false;

                return View();
            }
            else
            {
                return RedirectToAction("MenuPrincipal", "Inicio");
            }
        }

        [HttpPost]
        public ActionResult Index(string usuario, string contraseña)
        {
            UsuarioBO boUsuario = new UsuarioBO();
            ServicioLogin servicioLogin = new ServicioLogin();
            ServicioUsuario servicioUsuario = new ServicioUsuario();
            UsuarioBE usuarioEntrada = new UsuarioBE();
            UsuarioBE usuarioSalida = new UsuarioBE();
            usuarioEntrada.Usuario = usuario;
            usuarioEntrada.Contraseña = contraseña;
            int usuarioIntentos = -1;
            bool usuarioBloqueado = false;
            bool usuarioNoExiste = false;
            bool usuarioError = false;

            try
            {
                usuarioSalida = servicioLogin.IniciarSesion(usuarioEntrada); // Llamada al servicio
                if (usuarioSalida != null)
                {
                    // Puede logearse
                    Session["usuario"] = usuarioSalida;
                    // return RedirectToAction("MenuPrincipal", "Inicio");
                    return RedirectToAction("ListarExamenes", "Examen");
                }
                else
                {
                    usuarioError = true;
                    // Obtener usuario para saber la razón de por qué no puede logearse
                    // usuarioSalida = boUsuario.ObtenerUsuario(usuario);
                    usuarioSalida = servicioUsuario.ObtenerUsuario(usuario);
                    if (usuarioSalida != null)
                    {
                        usuarioBloqueado = usuarioSalida.Bloqueado;
                        usuarioIntentos = usuarioSalida.Intentos;
                    }
                    else
                    {
                        // No existe el usuario
                        usuarioNoExiste = true;
                    }
                }

                ViewBag.usuarioNoExiste = usuarioNoExiste;
                ViewBag.usuarioIntentos = usuarioIntentos;
                ViewBag.usuarioBloqueado = usuarioBloqueado;
                ViewBag.usuarioError = usuarioError;
            }
            catch(Exception ex)
            {
                ViewBag.mensajeError = ex.Message;
            }
                    

            return View(usuarioSalida);
        }
        
        public ActionResult CerrarSesion()
        {
            Session.Remove("usuario");
            Session["usuario"] = null;
            return RedirectToAction("Index");

        }

        public ActionResult RegistrarUsuario() {
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarUsuario(
            string Nombres,
            string ApellidoPaterno,
            string ApellidoMaterno,
            string Correo,
            string Dni,
            string Usuario, 
            string Contrasenia,
            HttpPostedFileBase imgUsuario
        ) {
            ServicioUsuario servicioUsuario = new ServicioUsuario();
            UsuarioBO bo = new UsuarioBO();

            UsuarioBE objUsu = new UsuarioBE();
            objUsu.Nombres = Nombres;
            objUsu.ApellidoPaterno = ApellidoPaterno;
            objUsu.ApellidoMaterno = ApellidoMaterno;
            objUsu.Correo = Correo;
            objUsu.Dni = Dni;
            objUsu.Usuario = Usuario;
            objUsu.Contrasenia = Contrasenia;
            objUsu.imgData = Utilitarios.ConversionImagen.ConvertirABase64(imgUsuario);
            objUsu.imgUsuario = null;

            // RespuestaBE respuesta = bo.RegistrarUsuario(objUsu);
            RespuestaBE respuesta = servicioUsuario.RegistrarUsuario(objUsu);
            if (respuesta.Registra)
            {
                EnvioCorreo.EnviarCorreo(objUsu, "Registro de Usuario", "Se ha registrado correctamente en la plataforma. Su usuario es: <b>" + objUsu.Usuario + "</b> y su clave es: <b>" + objUsu.Contrasenia + "</b>");
                ViewBag.mensajeRegistroCorrecto = "Se ha registrado correctamente";
                return RedirectToAction("Index");
            }
            else
            {
                if (respuesta.CodigoError == 2627)
                {
                    ViewBag.mensajeError = "El usuario ya se encuentra registrado";
                }
                else
                {
                    ViewBag.mensajeError = "Ocurrió un error al registrar";
                }
                
                return View();
            }
        }
    }
}