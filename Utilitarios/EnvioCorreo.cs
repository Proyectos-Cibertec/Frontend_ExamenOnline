using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace Utilitarios
{
    public class EnvioCorreo
    {
        public static bool EnviarCorreo(UsuarioBE usuario, string asunto, string mensaje)
        {
            bool enviado = false;
            string usuarioRemitente = ConfigurationManager.AppSettings["UsuarioRemitente"];
            string passwordRemitente = ConfigurationManager.AppSettings["PasswordRemitente"];
            string correoRemitente = usuarioRemitente;
            string contraseniaRemitente = passwordRemitente;
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com");

            try
            {
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                System.Net.NetworkCredential credentials =
                    new System.Net.NetworkCredential(correoRemitente, contraseniaRemitente);
                client.EnableSsl = true;
                client.Credentials = credentials;

                try
                {
                    var mail = new MailMessage();
                    mail.From = new MailAddress(correoRemitente); // Remitente
                    mail.IsBodyHtml = true;

                    string nombreDestinatario = usuario.ApellidoPaterno + " " + usuario.Nombres;
                    string correoDestinatario = usuario.Correo;
                    mail.To.Add(new MailAddress(correoDestinatario)); // Se va añadiendo los correos destinatarios

                    mail.Subject = asunto;
                    mail.Body = mensaje;
                    client.Send(mail);
                    enviado = true;
                }
                catch (Exception ex)
                {
                    enviado = false;
                    throw;
                }

            }
            catch (Exception ex)
            {
                enviado = false;
            }
            return enviado;
        }

    }
}
