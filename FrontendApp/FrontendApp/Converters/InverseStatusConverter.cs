using FrontendApp.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FrontendApp.Converters
{
    class InverseStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return "Unknown";
            }
            var x = (int)value;
            if(x == 0)
            {
                return "Sent friend request";
            }
            if (x == 1)
                return "Connected";
            else 
            {
                return "Accept Friend ";
            }
             
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
