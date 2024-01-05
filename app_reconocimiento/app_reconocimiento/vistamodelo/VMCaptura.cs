using app_reconocimiento.modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace app_reconocimiento.vistamodelo
{
    public class VMCaptura: BaseViewModel
    {
        private readonly IFlaskApiService flaskApiService;
        private ImageSource foto;
        public  ImageSource Foto
        {
            get { return foto; }
            set
            {
                foto = value;
                OnPropertyChanged(); // Asegúrate de tener la lógica de notificación de cambios
            }
        }

        private byte[] imageData;


        // Propiedad para el layout que mostrará los resultados de la detección
        private AbsoluteLayout layoutResultados;
        public AbsoluteLayout LayoutResultados
        {
            get { return layoutResultados; }
            set
            {
                layoutResultados = value;
                OnPropertyChanged(); // Asegúrate de tener la lógica de notificación de cambios
            }
        }

        public ICommand CapturarCommand => new Command(async () => await Capturar());

        public VMCaptura(IFlaskApiService flaskApiService)
        {
            this.flaskApiService = flaskApiService;
        }

        private async Task Capturar()
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.Camera>();

                if (status != PermissionStatus.Granted)
                {
                    // Maneja el caso donde el usuario no otorga permisos para la cámara
                    await DisplayAlert("Error", "Se requieren permisos de la cámara", "OK");
                    return;
                }

                var foto = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Capturar Foto"
                });

                if (foto != null)
                {
                    // Verifica que imageData no sea nulo
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (var stream = await foto.OpenReadAsync())
                        {
                            await stream.CopyToAsync(memoryStream);
                        }

                        imageData = memoryStream.ToArray();
                        string imagenBase64 = ConvertirImagenABase64(imageData);

                        // Verifica que flaskApiService no sea nulo
                        if (flaskApiService != null && imagenBase64 != null)
                        {
                            var boxes = await flaskApiService.DetectObjects(imagenBase64);
                            Console.WriteLine("Captura realizada");

                            // Muestra los resultados en la interfaz de usuario
                            MostrarResultados(boxes);
                        }
                        else
                        {
                            Console.WriteLine("flaskApiService es nulo.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("La foto capturada es nula.");
                }
            }
            catch (Exception ex)
            {
                // Maneja errores
                Console.WriteLine($"Error durante la captura: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

         private void MostrarResultados(List<List<object>> boxes)
         {
             // Aquí puedes implementar la lógica para mostrar los resultados en la interfaz de usuario

             // Supongamos que tienes un objeto de tipo Image llamado 'Foto' en tu vista
             Foto = ImageSource.FromStream(() => new MemoryStream(imageData));

             // Supongamos que tienes un objeto de tipo AbsoluteLayout llamado 'LayoutResultados' en tu vista
             // y que 'LayoutResultados' es una propiedad en tu clase CapturaViewModel
             LayoutResultados.Children.Clear();

             foreach (var box in boxes)
             {
                 // Obtén los valores de la caja
                 double x1 = Convert.ToDouble(box[0]);
                 double y1 = Convert.ToDouble(box[1]);
                 double x2 = Convert.ToDouble(box[2]);
                 double y2 = Convert.ToDouble(box[3]);
                 string etiqueta = Convert.ToString(box[4]);
                 double probabilidad = Convert.ToDouble(box[5]);

                 // Supongamos que tienes un objeto de tipo BoxView para representar la caja
                 var cajaView = new BoxView
                 {
                     Color = Color.Transparent,
                     BackgroundColor = Color.Transparent, // Usar BackgroundColor en lugar de Color
                     Margin = new Thickness(x1, y1, 0, 0),
                     WidthRequest = x2 - x1,
                     HeightRequest = y2 - y1
                 };

                 // Supongamos que tienes un objeto de tipo Label para mostrar la etiqueta y la probabilidad
                 var infoLabel = new Label
                 {
                     Text = $"{etiqueta} ({probabilidad:P2})",
                     TextColor = Color.Green,
                     FontSize = 12,
                     Margin = new Thickness(x1, y1 - 20, 0, 0)
                 };

                 // Agrega la caja y la etiqueta a LayoutResultados
                 LayoutResultados.Children.Add(cajaView, new Point(x1, y1));
                 LayoutResultados.Children.Add(infoLabel, new Point(x1, y1 - 20));
             }
         }

         
        private byte[] ObtenerDatosDeImagen()
        {
            // Implementa la lógica para obtener los datos de la imagen capturada
            // Puedes usar la API de Xamarin.Essentials MediaPicker para seleccionar o tomar una foto

            // Ejemplo utilizando MediaPicker para seleccionar una foto de la galería
            var opciones = new MediaPickerOptions
            {
                Title = "Seleccionar foto"
            };

            var foto = MediaPicker.PickPhotoAsync(opciones).Result;

            if (foto != null)
            {
                using (var fotoStream = foto.OpenReadAsync().Result)
                {
                    var memoryStream = new MemoryStream();
                    fotoStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }

            return null;
        }
        private string ConvertirImagenABase64(byte[] imageData)
        {
            if (imageData != null)
            {
                return Convert.ToBase64String(imageData);
            }

            return null;
        }
    }
}
