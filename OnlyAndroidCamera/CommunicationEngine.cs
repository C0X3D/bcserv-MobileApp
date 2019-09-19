using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace OnlyAndroidCamera
{
    class CommunicationEngine
    {
        internal async static Task<string> InsertCard(string imgPath)
        {
            
            return await Tests.ApiFunctions.UploadImage(imgPath); 

        }
    }
}