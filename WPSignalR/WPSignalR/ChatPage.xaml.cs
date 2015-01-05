using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace WPSignalR
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatPage : Page
    {
        public ChatPage()
        {
            this.InitializeComponent();

            Conversation conversation = new Conversation("Hendrik");
            conversation.addMessage(new Message("Hendrik", "Edwin", "Hallo wereld!!!!!!"));
            conversation.addMessage(new Message("Edwin", "Hendrik", "Hey Hendrik"));
            conversation.addMessage(new Message("Edwin", "Hendrik", "Mijn naam is Edwin."));
            conversation.addMessage(new Message("Hendrik", "Edwin", "Leuk voor je..."));
            conversation.addMessage(new Message("Edwin", "Hendrik", "Nou... lekker aardig zeg!!"));

            lbl_ContactName.Text = conversation.getUserId();
            lst_Messages.ItemsSource = conversation.getMessages();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// Sends the typed message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event data that describes how this event was triggered.</param>
        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            //connectionManager.sendMessage(txt_Message.Text);
            txt_Message.Text = "";
        }

        /// <summary>
        /// Resets the text if delivery failed and user didn't type anything new.
        /// </summary>
        /// <param name="text">The message that failed to be delivered.</param>
        public void sendMessageFailed(string text)
        {
            if (txt_Message.Text.Equals(""))
            {
                txt_Message.Text = text;
            }
        }
    }
}
