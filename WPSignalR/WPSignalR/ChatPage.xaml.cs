using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        IConnection connection;
        Conversation conversation;

        public ChatPage()
        {
            this.InitializeComponent();
            connection = SignalRConnection.Instance;

            conversation = connection.getConversations()[0];

            lbl_ContactName.Text = conversation.userId;
            this.DataContext = conversation;
            //lst_Messages.ItemsSource = conversation.messages;

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
            connection.sendMessage(new Message(
                connection.getMyUserId(),
                connection.getMyUserId(), 
                //conversation.getUserId(),
                txt_Message.Text)
            );
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
