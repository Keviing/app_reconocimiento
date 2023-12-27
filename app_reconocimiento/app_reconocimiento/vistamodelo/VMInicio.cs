using app_reconocimiento.vista;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace app_reconocimiento.vistamodelo
{
    internal class VMInicio:BaseViewModel
    {
        #region Variables
        #endregion

        #region Constructor
        public VMInicio(INavigation navigation) { 
        
            Navigation = navigation;
        }
        #endregion

        #region OBJETOS
        #endregion

        #region PROCESOS

        public async Task iniciar()
        {
            await Navigation.PushAsync(new Opciones());
        }
        #endregion

        #region COMANDOS

        public ICommand IniciarCommand => new Command(async () => await iniciar());
        #endregion
    }
}
