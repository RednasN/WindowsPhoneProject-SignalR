using System;
using System.Collections.Generic;

namespace WPSignalR
{
    class location
    {
        private int userId { get; set; }
        private double latitude { get; set; }
        private double longitude { get; set; }
    }

    class conversation
    {
        private int userId { get; set; }
        private List<string> messages { get; }

        public void addMessage(string message) {
            messages.Add(message);
        }
    }
}
