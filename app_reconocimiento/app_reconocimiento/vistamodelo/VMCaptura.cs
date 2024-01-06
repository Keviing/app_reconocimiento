using app_reconocimiento.modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace app_reconocimiento.vistamodelo
{
    public class VMCaptura: BaseViewModel
    {
        private ImageSource _capturedImage;
        public ImageSource CapturedImage
        {
            get => _capturedImage;
            set => SetProperty(ref _capturedImage, value);
        }

        public ICommand OpenCameraCommand { get; }

        public VMCaptura()
        {
            OpenCameraCommand = new Command(async () => await OpenCameraAsync());
        }
      
        // LOGICA PARA ABRIR LA CAMARA
        private async Task OpenCameraAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo == null)
                    return;

                using (var stream = await photo.OpenReadAsync())
                {
                    CapturedImage = ImageSource.FromStream(() => stream);

                    // Convertir y enviar la imagen
                    var base64Image = ConvertImageToBase64(stream);
                    var serverResponse = await SendImageToServer(base64Image);
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones (p.ej., permisos de cámara no concedidos)
                await DisplayAlert("Error", "No se pudo abrir la cámara: " + ex.Message, "OK");
            }
        }
        private string ConvertImageToBase64(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            string base64ImageRepresentation = Convert.ToBase64String(buffer); 
            return base64ImageRepresentation;
        }

        private async Task<string> SendImageToServer(string base64Image)
        {
            var httpClient = new HttpClient();
            var jsonRequest = JsonConvert.SerializeObject(new { image = base64Image });
            Debug.WriteLine("JSON que se enviará: " + jsonRequest);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("http://192.168.100.73:4000/detect", content);

            if (response.IsSuccessStatusCode)
            {
                var serverResponse = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("Respuesta del servidor: " + serverResponse);
                return serverResponse;
            }
            return null;
        }
    }
}
