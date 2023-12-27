using app_reconocimiento.vistamodelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace app_reconocimiento.vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Opciones : ContentPage
	{
		public Opciones ()
		{
			InitializeComponent ();
			BindingContext = new VMOpciones(Navigation);
		}

        private void Button_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}