using app_reconocimiento.modelo;
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
	public partial class DetalleSenial : ContentPage
	{
		public DetalleSenial (SenialModel senial)
		{
			InitializeComponent ();
			BindingContext = new VMDetalleSenial(senial);
		}
	}
}