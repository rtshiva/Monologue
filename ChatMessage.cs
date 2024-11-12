using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.ComponentModel;
using Windows.UI;

namespace ChatWinUi
{
    public partial class ChatMessage : INotifyPropertyChanged
    {
        private string text;
        private bool isLocalUser;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public bool IsLocalUser
        {
            get => isLocalUser;
            set
            {
                isLocalUser = value;
                OnPropertyChanged(nameof(IsLocalUser));
                OnPropertyChanged(nameof(SwitchUserMenuText));
                OnPropertyChanged(nameof(Background));
                OnPropertyChanged(nameof(BorderBrush));
                OnPropertyChanged(nameof(HorizontalAlignment));
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Username
        {
            get
            {
                return IsLocalUser ? "User" : "Other";
            }
        }

        public string SwitchUserMenuText => IsLocalUser ?
            "Switch to Other User" : "Switch to Local User";

        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return isLocalUser ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            }
        }

        public Brush Background
        {
            get
            {
                return GetMessageBackground(IsLocalUser);
            }
        }

        public Brush BorderBrush
        {
            get
            {
                return GetMessageBorder(IsLocalUser);
            }
        }


        private SolidColorBrush GetMessageBackground(bool isLocalUser)
        {
            return new SolidColorBrush(isLocalUser ?
                Color.FromArgb(255, 220, 248, 198) :  // Light green
                Color.FromArgb(255, 207, 225, 254));  // Light blue
        }

        private SolidColorBrush GetMessageBorder(bool isLocalUser)
        {
            return new SolidColorBrush(isLocalUser ?
                Color.FromArgb(255, 190, 238, 158) :  // Darker green
                Color.FromArgb(255, 187, 205, 234));  // Darker blue
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void ToggleUser()
        {
            IsLocalUser = !isLocalUser;
        }
    }


    public class UserColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ?
                new SolidColorBrush(Microsoft.UI.Colors.LightGreen) :
                new SolidColorBrush(Microsoft.UI.Colors.LightBlue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class AlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
