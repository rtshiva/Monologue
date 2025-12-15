using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Core;
using WinRT.Interop;
using ChatWinUi.Dialogs;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChatWinUi
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TabPage : Page
    {
        private ObservableCollection<ChatMessage> Messages { get; } = new();

        public TabPage()
        {
            this.InitializeComponent();
            var isDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
            ChatScroller.ItemsSource = Messages;
            MessageBox.Focus(FocusState.Programmatic);
            
            if(isDark)
            {
                UserButton.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 22, 120, 14));
                OtherButton.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 41, 153, 186));
            }
            else
            {
                UserButton.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 220, 248, 198));
                OtherButton.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 207, 225, 254));
            }

        }

        private void AddMessage(string text, bool isLocalUser)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var message = new ChatMessage
            {
                Text = text,
                IsLocalUser = isLocalUser
            };

            Messages.Add(message);
            MessageBox.Text = string.Empty;
            MessageBox.Focus(FocusState.Programmatic);

            // Scroll to the bottom
            //ChatScroller.ChangeView(null, ChatScroller.ScrollableHeight, null);
            ChatScroller.ScrollIntoView(message);
        }


        private void OtherButton_Click(object sender, RoutedEventArgs e)
        {
            AddMessage(MessageBox.Text, false);
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            AddMessage(MessageBox.Text, true);
        }


        private void MessageBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // if alt = enter is pressed call addmessage with false
            var ctrlState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control);
            bool isCtrlDown = (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            if (isCtrlDown && e.Key == VirtualKey.Enter)
            {
                // Handle Alt+Enter key combination
                AddMessage(MessageBox.Text, false);
                e.Handled = true;
            }
            else if (e.Key == Windows.System.VirtualKey.Enter)
            {
                AddMessage(MessageBox.Text, true);
                e.Handled = true;
            }
        }

        private void SwitchUser_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            var message = menuItem.DataContext as ChatMessage;
        }

        private void Message_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            var flyout = grid.ContextFlyout as MenuFlyout;
            flyout.ShowAt(grid, e.GetPosition(grid));

        }

        private void Message_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ChatMessage SelectedMessage = ChatScroller.SelectedItem as ChatMessage;
            var ctrlState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control);
            bool isCtrlDown = (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            if (isCtrlDown)
            {
                // if user pressed ctrl & clicked on any message we toggle the user
                ToggleMessageUser(SelectedMessage, false);
                return;
            }

            var deleteState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Delete);
            bool isDelDown = (deleteState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            if (isDelDown)
            {
                ToggleMessageUser(SelectedMessage, true);
                return;
            }
        }

        private void ToggleMessageUser(ChatMessage SelectedMessage, bool delete)
        {
            // Find the index of the tapped item

            if (SelectedMessage == null)
            {
                return;
            }
            int index = ChatScroller.SelectedIndex;
            // only way to update already inserted items in UI was to remove and insert them back 
            Messages.RemoveAt(index);
            if (delete)
            {
                return;
            }
            SelectedMessage.ToggleUser();
            Messages.Insert(index, SelectedMessage);
            Brush bbrush = SelectedMessage.Background;
            ChatScroller.SelectedIndex = -1;
        }

        private async void EmojiButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EmojiPickerDialog();
            dialog.XamlRoot = this.XamlRoot;
            var result = await dialog.ShowAsync();

            if (!string.IsNullOrEmpty(dialog.SelectedEmoji))
            {
                var caretIndex = MessageBox.SelectionStart;
                MessageBox.Text = MessageBox.Text.Insert(caretIndex, dialog.SelectedEmoji);
                MessageBox.SelectionStart = caretIndex + dialog.SelectedEmoji.Length;
            }
            MessageBox.Focus(FocusState.Programmatic);
        }

        [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.Serialize<TValue>(TValue, JsonSerializerOptions)")]
        internal async Task ExportChatAsync(string TabTitle, IntPtr Hwnd)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = $"{TabTitle}.json"
            };

            savePicker.FileTypeChoices.Add("JSON files", new List<string>() { ".json" });

            // Initialize the FileSavePicker with the window handle
            InitializeWithWindow.Initialize(savePicker, Hwnd);

            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                var chatData = Messages.Select(m => new
                {
                    m.Username,
                    m.Text
                });

                var json = System.Text.Json.JsonSerializer.Serialize(chatData, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await FileIO.WriteTextAsync(file, json);
            }
        }

        private void Page_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void MessageBox_Loaded(object sender, RoutedEventArgs e)
        {
            // focus can be set only after textbox is loaded, hence done here
            MessageBox.Focus(FocusState.Programmatic);
        }

        internal string LoadChat(nint hwnd)
        {
            string fileName = "LoadedChat";
            // open file picker to load chat data , restrict to file options to json files only
            try
            {
                FileOpenPicker fileOpenPicker = new FileOpenPicker
                {
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                };
                fileOpenPicker.FileTypeFilter.Add(".json");
                InitializeWithWindow.Initialize(fileOpenPicker, hwnd);
                var file = fileOpenPicker.PickSingleFileAsync().GetAwaiter().GetResult();
                if (file != null)
                {
                    // read the text from the file
                    var json = FileIO.ReadTextAsync(file).GetAwaiter().GetResult();
                    // Deserialize the JSON content to a list of SerializedChatMessage objects
                    List<SerializedChatMessage> loadedmsgs = JsonConvert.DeserializeObject<List<SerializedChatMessage>>(json);
                    foreach (var msg in loadedmsgs)
                    {
                        Messages.Add(new ChatMessage(msg));
                    }
                    fileName = file.Name;
                }
            }
            catch (Exception e)
            {
                return "";
            }
            return fileName;
        }

        private void ChatScroller_Loaded(object sender, RoutedEventArgs e)
        {
            // if we loaded messages from json, we need to scroll to the last message
            if (Messages.Count > 0)
                ChatScroller.ScrollIntoView(Messages.Last());
        }
    }
}
