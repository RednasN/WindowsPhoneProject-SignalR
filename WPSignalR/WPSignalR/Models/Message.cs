namespace WPSignalR
{
    public class Message
    {
        public Message(string senderId, string receiverId, string text)
        {
            this.senderId = senderId;
            this.receiverId = receiverId;
            this.text = text;
        }

        public enum Position
        {
            Left,
            Right
        }
        public Position position { get; set; }

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
        public string senderId { get; set; }
        public string receiverId { get; set; }
        public string text { get; set; }
    }
}
