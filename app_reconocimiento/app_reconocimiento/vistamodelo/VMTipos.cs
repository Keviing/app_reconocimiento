using app_reconocimiento.modelo;
using app_reconocimiento.vista;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace app_reconocimiento.vistamodelo
{
    public class VMTipos : BaseViewModel
    {
        private ObservableCollection<Grouping<string, SenialModel>> senialesAgrupadas;

        public ObservableCollection<Grouping<string, SenialModel>> SenialesAgrupadas
        {
            get { return senialesAgrupadas; }
            set { SetProperty(ref senialesAgrupadas, value); }
        }

        public VMTipos(INavigation navigation)
        {
            Navigation = navigation;
            SenialesAgrupadas = new ObservableCollection<Grouping<string, SenialModel>>();
            CargarSeniales();
        }

        private SenialModel senialSeleccionada;
        public SenialModel SenialSeleccionada
        {
            get => senialSeleccionada;
            set
            {
                if (value != null && SetProperty(ref senialSeleccionada, value))
                {
                    MostrarDetalleSenial(value);
                }
            }
        }

        private async void MostrarDetalleSenial(SenialModel senial)
        {
            // Aquí implementar la navegación a la vista de detalles
            await Navigation.PushAsync(new DetalleSenial(senial));
            SenialSeleccionada = null; // Resetear la selección
        }
        private async Task CargarSeniales()
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = "https://us-central1-swtesis-e0343.cloudfunctions.net/app/api/seniales";
                string response = await client.GetStringAsync(url);

                var senialesList = JsonConvert.DeserializeObject<List<SenialModel>>(response);

                var senialesAgrupadasList = senialesList
                .GroupBy(s => new { s.Id_Clase, s.Tipo_Senial })
            .Select(g => new Grouping<string, SenialModel>(g.Key.Id_Clase, g.Key.Tipo_Senial, g))
            .ToList();

                foreach (var grupo in senialesAgrupadasList)
                {
                    SenialesAgrupadas.Add(grupo);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar las señales: {ex.Message}", "OK");
            }
        }

        public async Task Retroceder()
        {
            await Navigation.PopAsync();
        }

        public ICommand RetrocederCommand => new Command(async () => await Retroceder());

    }

    // Clase auxiliar para agrupar elementos.
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Id_Clase { get; private set; }
        public string Tipo_Senial { get; private set; }

        public Grouping(K idClase, string tipoSenial, IEnumerable<T> items)
        {
            Id_Clase = idClase;
            Tipo_Senial = tipoSenial;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
