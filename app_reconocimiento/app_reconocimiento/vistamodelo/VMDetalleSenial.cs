using app_reconocimiento.modelo;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace app_reconocimiento.vistamodelo
{
    class VMDetalleSenial
    {
        private SenialModel senial;

        public VMDetalleSenial(SenialModel senial)
        {
            this.senial = senial;
        }
        public string Nombre => senial.Nombre;
        public string Descripcion => senial.Descripcion;
        public ImageSource Imagen => senial.Imagen;
    }
}
