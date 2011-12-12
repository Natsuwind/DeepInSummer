using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Common
{
    public class AsyncObject
    {
        public object StateId { get; set; }
        public object MySetId { get; set; }
        public object MySetObject { get; set; }

        public AsyncObject(object stateId, object mySetId, object mySetObject)
        {
            this.StateId = stateId;
            this.MySetId = mySetId;
            this.MySetObject = mySetObject;
        }
    }
}
