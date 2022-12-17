using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Monologue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // list to keep track of all message boubles! can use to format the content anytime
        List<Border> Messagelist = new List<Border>();
        Color MyColor = Colors.LightGreen;
        Color OtherColor = Colors.LightSkyBlue;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void pushMe_Click(object sender, RoutedEventArgs e)
        {
            PushText(sender, e, false);
        }

        private void PushText(object sender, RoutedEventArgs e, bool LeftMessage)
        {
            TextBlock MessageObj = new TextBlock();
            Border BorderObj = new Border();
            BorderObj.HorizontalAlignment = (LeftMessage ? HorizontalAlignment.Left : HorizontalAlignment.Right);
            BorderObj.Background = (LeftMessage ? new SolidColorBrush(OtherColor) : new SolidColorBrush(MyColor));
            BorderObj.BorderThickness = new Thickness(3);
            BorderObj.CornerRadius = new CornerRadius(10);
            BorderObj.Margin = new Thickness(0, 0, 0, 0);
            BorderObj.Padding = new Thickness(4);

            MessageObj.TextWrapping = TextWrapping.Wrap;
            MessageObj.Text = InputBox.Text;
            BorderObj.MaxWidth= MainWindowTest.Width - 30;
            InputBox.Text = "";
            
            BorderObj.Child = MessageObj;
            BorderObj.MouseDown += MouseClick_handler;
            Messagelist.Add(BorderObj);
            MessageViewer.Children.Add(BorderObj);
        }

        private void MainWindowTest_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(Messagelist.Count > 0) 
            {
                foreach (Border Obj in Messagelist)
                {
                    Obj.MaxWidth= MainWindowTest.Width - 30;
                }
            }
        }

        private void MouseClick_handler(object sender, MouseButtonEventArgs e)
        {
            Border Message = (Border)sender;
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (Message.HorizontalAlignment == HorizontalAlignment.Left)
                {
                    Message.HorizontalAlignment = HorizontalAlignment.Right;
                    Message.Background = new SolidColorBrush(MyColor);
                }
                else
                {
                    Message.HorizontalAlignment = HorizontalAlignment.Left;
                    Message.Background = new SolidColorBrush(OtherColor);
                }
            }

        }

        private void pushOtherMe_Click(object sender, RoutedEventArgs e)
        {
            PushText(sender, e, true);
        }

        private void keydown_handler(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.OemPlus)
            {
                IncreaseWindowFont();
            }
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.OemMinus)
            {
                DecreaseWindowFont();
            }

        }

        private void IncreaseWindowFont()
        {
            MainWindowTest.FontSize = MainWindowTest.FontSize + 1;
        }
        private void DecreaseWindowFont()
        {
            MainWindowTest.FontSize = MainWindowTest.FontSize - 1;
        }

        private void mousescroller_handler(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Delta > 0)
                {
                    IncreaseWindowFont();
                }
                if (e.Delta < 0)
                {
                    DecreaseWindowFont();
                }
            }
        }
    }
}
