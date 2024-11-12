using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using Windows.Foundation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChatWinUi
{
    public sealed partial class EmojiPicker : UserControl
    {
        public string SelectedEmoji { get; private set; }

        public event TypedEventHandler<EmojiPicker, string> EmojiSelected;

        public EmojiPicker()
        {
            InitializeComponent();

            // Add some common emojis
            var emojis = new List<string>
            {
                "😊", "😂", "🤣", "❤️", "😍", "👍", "😒", "😭", "😘", "🥰",
                "😅", "😁", "👌", "😉", "🙂", "🤦‍♂️", "🤦‍♀️", "🎉", "✨", "🌟"
            };

            EmojiList.ItemsSource = emojis;
        }

        private void EmojiButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                SelectedEmoji = button.Content.ToString();
                EmojiSelected?.Invoke(this, SelectedEmoji);
            }
        }

    }
}




