using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace ChatWinUi
{
    public class SerializedChatMessage
    {
        public string Username;
        public string Text;
    }

    public partial class ChatMessage : INotifyPropertyChanged
    {
        private string text;
        private bool isLocalUser;
        private static ThemeBrushHelper brushHelper = new ThemeBrushHelper();

        public ChatMessage(SerializedChatMessage msg)
        {
            Text = msg.Text;
            IsLocalUser = msg.Username == "User";
        }

        public ChatMessage(string text, bool isLocalUser)
        {
            Text = text;
            IsLocalUser = isLocalUser;
        }

        public ChatMessage()
        {

        }

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
            return isLocalUser ? brushHelper.userBackground : brushHelper.otherBackground;
        }

        private SolidColorBrush GetMessageBorder(bool isLocalUser)
        {
            return isLocalUser ? brushHelper.userBorder : brushHelper.otherBorder;
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
        public SerializedChatMessage Serialize()
        {
            return new SerializedChatMessage
            {
                Username = Username,
                Text = Text
            };
        }

        private class ThemeBrushHelper
        {
            public SolidColorBrush userBackground;
            public SolidColorBrush otherBackground;
            public SolidColorBrush userBorder;
            public SolidColorBrush otherBorder;

            public ThemeBrushHelper()
            {
                userBackground = new SolidColorBrush();
                otherBackground = new SolidColorBrush();
                userBorder = new SolidColorBrush();
                otherBorder = new SolidColorBrush();
                UpdateBrushes();
                var settings = new UISettings();
                settings.ColorValuesChanged += (s, e) => UpdateBrushes();
            }

            private void UpdateBrushes()
            {
                var isDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;

                userBackground.Color = isDark ? Color.FromArgb(255, 22, 120, 14) : Color.FromArgb(255, 220, 248, 198);
                otherBackground.Color = isDark ? Color.FromArgb(255, 41, 153, 186) : Color.FromArgb(255, 207, 225, 254);
                userBorder.Color = isDark ? Color.FromArgb(255, 18, 90, 12) : Color.FromArgb(255, 190, 238, 158);
                otherBorder.Color = isDark ? Color.FromArgb(255, 50, 50, 50) : Color.FromArgb(255, 187, 205, 234);
            }
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
