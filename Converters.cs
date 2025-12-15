using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using Microsoft.UI.Xaml;

namespace ChatWinUi
{
    public class MessageBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isLocalUser = (bool)value;
            if (isLocalUser)
            {
                // Light green for local user
                return (SolidColorBrush)Application.Current.Resources["SystemAccentColorLight1"];
            }
            else
            {
                // Light blue/gray for other user
                return (SolidColorBrush)Application.Current.Resources["SystemBaseLowColor"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class MessageBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isLocalUser = (bool)value;
            if (isLocalUser)
            {
                // Darker green for local user border
                return (SolidColorBrush)Application.Current.Resources["SystemAccentColorLight2"];
            }
            else
            {
                // Darker blue/gray for other user border
                return (SolidColorBrush)Application.Current.Resources["SystemBaseMediumLowColor"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

