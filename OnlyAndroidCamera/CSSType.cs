using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace OnlyAndroidCamera
{
    public static class CSSType
    {
        public static GradientDrawable gd = new GradientDrawable(
           GradientDrawable.Orientation.BottomTop,
           new int[] { 0xFF6162, 0xFF1313 });
    }
}