using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace app_reconocimiento.modelo
{
    public interface IFlaskApiService
    {
        Task<List<List<object>>> DetectObjects(string base64Image);


    }
}
