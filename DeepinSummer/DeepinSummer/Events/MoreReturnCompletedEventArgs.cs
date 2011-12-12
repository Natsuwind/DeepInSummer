using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Events
{
    public class MoreReturnCompletedEventArgs : ReturnCompletedEventArgs
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public MoreReturnCompletedEventArgs(
            bool isSuccess,
            string message,
            object returnObj,
            Exception e,
            bool canceled,
            object state)
            : base(returnObj, e, canceled, state)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.ReturnObject = returnObj;
        }
    }
}
