using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utilitarios
{
    public class ConversionImagen
    {
        public static string ConvertirABase64(HttpPostedFileBase imagen)
        {
            byte[] imagenOriginal = new byte[0];
            string ImagenDataURL64 = "";
            try
            {
                if (imagen != null)
                {
                    int tamanio = imagen.ContentLength;
                    imagenOriginal = new byte[tamanio];
                    imagen.InputStream.Read(imagenOriginal, 0, tamanio);

                    System.Drawing.Bitmap ImagenOriginalBinaria = new System.Drawing.Bitmap(imagen.InputStream);
                    ImagenDataURL64 = "data:image/png;base64," + Convert.ToBase64String(imagenOriginal);
                }
            }
            catch (Exception ex)
            {
                ImagenDataURL64 = "";
            }
            return ImagenDataURL64;
        }
    }
}
