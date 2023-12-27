using app_reconocimiento.modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        private async Task CargarSeniales()
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = "https://us-central1-swtesis-e0343.cloudfunctions.net/app/api/seniales";
                string response = await client.GetStringAsync(url);

                var senialesList = JsonConvert.DeserializeObject<List<SenialModel>>(response);

                var senialesAgrupadasList = senialesList
                    .GroupBy(s => s.Id_Clase)
                    .Select(g => new Grouping<string, SenialModel>(g.Key, g))
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
    }

    // Clase auxiliar para agrupar elementos.
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
