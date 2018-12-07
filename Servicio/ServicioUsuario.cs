using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Servicio
{
    public class ServicioUsuario
    {
        public UsuarioBE ObtenerUsuario(string usuario)
        {
            UsuarioBE oUsuario = null;
            var webAddr = "http://localhost:8080/RestExamenOnline/rest/servicioUsuario/obtenerUsuario/";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";

            // Se envía el objeto usuario en formato Json
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(usuario);
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

        

        public RespuestaBE RegistrarUsuario(UsuarioBE usuario)
        {
            RespuestaBE respuesta = null;
            var webAddr = "http://localhost:8080/RestExamenOnline/rest/servicioUsuario/registrarUsuario/";
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
                respuesta = JsonConvert.DeserializeObject<RespuestaBE>(result);
            }

            return respuesta;
        }
    }
}
