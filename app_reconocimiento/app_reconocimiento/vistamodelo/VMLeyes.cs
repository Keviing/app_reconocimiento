using app_reconocimiento.vista;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace app_reconocimiento.vistamodelo
{
    internal class VMLeyes:BaseViewModel
    {

        #region Constructor
        public VMLeyes(INavigation navigation)
        {

            Navigation = navigation;
        }
        #endregion

        #region PROCESOS

        public async Task Retroceder()
        {
            await Navigation.PopAsync();
        }

        
        #endregion

        #region COMANDOS

        public ICommand RetrocederCommand => new Command(async () => await Retroceder());
        
        #endregion

    }
}
