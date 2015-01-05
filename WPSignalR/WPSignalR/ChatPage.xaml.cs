using System.Collections.Generic;
using System.Linq;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            connection = SignalRConnection.Instance;

            List<Conversation> list = connection.conversations.ToList<Conversation>();
            int conversationIndex = list.FindIndex(x => x.userId == (string)e.Parameter);

            if (conversationIndex >= 0)
            {
                conversation = connection.conversations[conversationIndex];

                lbl_PageName.Text = conversation.userId;
                this.DataContext = conversation;
            }
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
                conversation.userId,
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
