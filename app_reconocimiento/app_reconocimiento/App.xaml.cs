using app_reconocimiento.vista;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace app_reconocimiento
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage( new Inicio());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
