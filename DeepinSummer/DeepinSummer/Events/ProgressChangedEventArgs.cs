using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Events
{
    public class ProgressChangedEventArgs : EventArgs
    {
        public int CurrentPercent { get; set; }
        public int TotalPercent { get; set; }
        public object UserState { get; set; }

        public ProgressChangedEventArgs(int currentPercent, int totalPercent, object userState)
        {
            this.CurrentPercent = currentPercent;
            this.TotalPercent = totalPercent;
            this.UserState = userState;
        }
    }
}
