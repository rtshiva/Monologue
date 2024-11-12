// MainWindow.xaml.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.Graphics;
using WinRT.Interop;

namespace ChatWinUi
{
    public sealed partial class MainWindow : Window
    {
        private int tabCounter = 2;
        public IntPtr Hwnd { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            this.AppWindow.SetIcon("Assets\\icon_talk80.ico");
            // Initialize window handle and AppWindow
            Hwnd = WindowNative.GetWindowHandle(this);
            AppWindow.Resize(new SizeInt32(800, 600));

        }


        private void AddNewTab_Click(TabView sender, object args)
        {
            AddNewTab();
        }

        private void AddNewTab()
        {
            Frame frame = new Frame();
            var newTab = new TabViewItem
            {
                Header = $"Chat {tabCounter++}",
                Content = frame,
            };
            frame.Navigate(typeof(TabPage));
            ChatTabs.TabItems.Add(newTab);
            ChatTabs.SelectedItem = newTab;
        }

        private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }

        private async void RenameTab_Click(object sender, RoutedEventArgs e)
        {
            var selectedTab = ChatTabs.SelectedItem as TabViewItem;
            if (selectedTab == null) return;

            var dialog = new ContentDialog
            {
                Title = "Rename Tab",
                PrimaryButtonText = "OK",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = new TextBox { PlaceholderText = "Enter new tab name", Text = selectedTab.Header.ToString(), SelectionLength = selectedTab.Header.ToString().Length },
                XamlRoot = Content.XamlRoot
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var newName = (dialog.Content as TextBox).Text;
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    selectedTab.Header = newName;
                }
            }
        }

        private void ExportChat_Click(object sender, RoutedEventArgs e)
        {
            // Get the current active tab
            var selectedTab = ChatTabs.SelectedItem as TabViewItem;
            if (selectedTab == null) return;

            // Get the Frame from the current tab's Content
            var frame = selectedTab.Content as Frame;
            if (frame == null) return;

            // Get the TabPage from the Frame's Content
            var page = frame.Content as TabPage;
            if (page == null) return;

            // Call the ExportChat method on the TabPage
            _ = page.ExportChatAsync(selectedTab.Header.ToString(), Hwnd);
        }

        private void LoadChat_Click(object sender, RoutedEventArgs e)
        {
            AddNewTab();

            // Get the current active tab
            var selectedTab = ChatTabs.SelectedItem as TabViewItem;
            if (selectedTab == null) return;

            // Get the Frame from the current tab's Content
            var frame = selectedTab.Content as Frame;
            if (frame == null) return;

            // Get the TabPage from the Frame's Content
            var page = frame.Content as TabPage;
            if (page == null) return;

            // Call the ExportChat method on the TabPage
            selectedTab.Header = page.LoadChat(Hwnd);
        }
    }
}
