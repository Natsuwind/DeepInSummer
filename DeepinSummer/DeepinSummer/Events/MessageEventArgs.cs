using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Events
{
    public class MessageEventArgs: EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string ExtMessage { get; set; }
        public object UserState { get; set; }

        public MessageEventArgs(string title, string message, string extMessage, object userState)
        {
            this.Title = title;
            this.Message = message;
            this.ExtMessage = extMessage;
            this.UserState = userState;
        }
    }
}
