using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Events
{
    public class ReturnCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        public object ReturnObject { get; set; }

        public ReturnCompletedEventArgs(
            object returnObj,
            Exception e,
            bool canceled,
            object state)
            : base(e, canceled, state)
        {
            this.ReturnObject = returnObj;
        }
    }
}
