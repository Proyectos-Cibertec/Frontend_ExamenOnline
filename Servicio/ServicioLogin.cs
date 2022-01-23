using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;

namespace Servicio
{
    public class ServicioLogin
    {
        public UsuarioBE IniciarSesion(UsuarioBE usuario)
        {
            UsuarioBE oUsuario = null;
            string webAddr = ConfigurationManager.AppSettings["ApiExamenOnlineEndpointBase"] + ConfigurationManager.AppSettings["PathIniciarSesion"];
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";

            // Se envía el objeto usuario en formato Json
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(usuario);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            // Se recibe la respuesta del servicio en formato Json
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                // Convierte el json en UsuarioBE
                oUsuario = JsonConvert.DeserializeObject<UsuarioBE>(result);
            }

            return oUsuario;
        }
    }
}
