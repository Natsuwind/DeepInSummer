using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Events
{
    public class MoreProgressChangedEventArgs : ProgressChangedEventArgs
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Exception Error { get; set; }
        public MoreProgressChangedEventArgs(bool isSuccess, string message, Exception error, int currentPercent, int totalPercent, object userState)
            : base(currentPercent, totalPercent, userState)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Error = error;
        }
    }
}
