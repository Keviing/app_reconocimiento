using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace app_reconocimiento.modelo
{
    public class FlaskApiService: IFlaskApiService
    {
        private readonly string apiUrl = "http://10.52.106.164:4000";

        public async Task<List<List<object>>> DetectObjects(string base64Image)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var content = new MultipartFormDataContent();

                    // Convertir la cadena base64 a un array de bytes
                    byte[] imageData = Convert.FromBase64String(base64Image);

                    var imageContent = new ByteArrayContent(imageData);

                    // Puedes ajustar el nombre del parámetro según la implementación en tu servidor Flask
                    content.Add(imageContent, "image_file");

                    var response = await httpClient.PostAsync($"{apiUrl}/detect", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<List<List<object>>>();
                        return result;
                    }
                    else
                    {
                        // Manejar el caso en que la solicitud no sea exitosa
                        Console.WriteLine($"Error en la solicitud: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar otros errores
                Console.WriteLine($"Error durante la detección: {ex.Message}");
            }

            return null;
        }

    }
}
