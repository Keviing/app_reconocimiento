using app_reconocimiento.vista;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace app_reconocimiento.vistamodelo
{
internal class VMOpciones:BaseViewModel
{

    #region Variables
    #endregion

    #region Constructor
    public VMOpciones(INavigation navigation)
    {

        Navigation = navigation;
    }
    #endregion

    #region OBJETOS
    #endregion

    #region PROCESOS

    public async Task Retroceder()
    {
        await Navigation.PopAsync();
    }

    public void IrCaptura()
    {
        Navigation.PushAsync(new Captura());
    }
    public void IrTipos()
    {
        Navigation.PushAsync(new Tipos());

    }

    public void IrLeyes()
    {
        Navigation.PushAsync(new Leyes());


    }
    #endregion

    #region COMANDOS

    public ICommand RetrocederCommand => new Command(async () => await Retroceder());
    public ICommand IrCapturaCommand => new Command( IrCaptura);
    public ICommand IrTiposCommand => new Command( IrTipos);
    public ICommand IrLeyesCommand => new Command(IrLeyes);
    #endregion
}
}

