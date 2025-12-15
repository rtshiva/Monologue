using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace ChatWinUi.Dialogs
{
    public sealed partial class EmojiPickerDialog : ContentDialog
    {
        public string SelectedEmoji { get; private set; }

        public EmojiPickerDialog()
        {
            this.InitializeComponent();
        }

        private void EmojiPicker_EmojiSelected(EmojiPicker sender, string emoji)
        {
            SelectedEmoji = emoji;
            this.Hide();
        }
    }
}
