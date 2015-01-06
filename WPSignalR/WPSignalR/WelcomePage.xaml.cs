using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace WPSignalR
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        SignalRConnection connection;
        public WelcomePage()
        {
            this.InitializeComponent();

            try
            {
                connection = SignalRConnection.Instance;
            }
            catch
            {
                // @TODO: Do some proper logging.
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btn_RegisterUsername_Click(object sender, RoutedEventArgs e)
        {
            if (txt_Username.Text.Length > 0)
            {
                connection.registerUserName(txt_Username.Text);
				Frame.Navigate(typeof(MainPage));
            }
        }
        private void OnKeyDownHandler(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                btn_RegisterUsername_Click(sender, new RoutedEventArgs());
            }
        }
    }
}
