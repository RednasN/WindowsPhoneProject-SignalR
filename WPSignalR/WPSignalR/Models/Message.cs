namespace WPSignalR
{
    public class Message
    {
        public string senderId { get; set; }
        public string receiverId { get; set; }
        public string text { get; set; }
        public Position position { get; set; }

        /// <summary>
        /// Message constructor, multiple params are required to create a instance.
        /// </summary>
        /// <param name="senderId">The userId of the sender</param>
        /// <param name="receiverId">The userId of the receiver</param>
        /// <param name="text">The message text</param>
        public Message(string senderId, string receiverId, string text)
        {
            this.senderId = senderId;
            this.receiverId = receiverId;
            this.text = text;
        }

        /// <summary>
        /// Position enum, either Left of Right.
        /// </summary>
        public enum Position
        {
            Left,
            Right
        }

        /// <summary>
        /// Alignment is a string conversion of the Position enum. It indicates whether the message is supposed to show Left or Right.
        /// </summary>
        public string alignment
        {
            get
            {
                if (this.position == Position.Left)
                {
                    return "Left";
                }
                else
                {
                    return "Right";
                }
            }
        }

    }
}
