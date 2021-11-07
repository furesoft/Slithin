using System;
using Slithin.Core;

namespace Slithin.Messages
{
    public class PostActionMessage : AsynchronousMessage
    {
        public PostActionMessage(Action action)
        {
            Action = action;
        }

        public Action Action { get; set; }
    }
}
