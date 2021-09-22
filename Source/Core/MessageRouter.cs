using System;
using System.Collections.Generic;

namespace Slithin.Core
{
    public class MessageRouter
    {
        private readonly Dictionary<Type, Action<object>> _handlers = new();

        public void Register<T>(Action<T> handler)
            where T : AsynchronousMessage
        {
            if (!_handlers.ContainsKey(typeof(T)))
            {
                _handlers.Add(typeof(T), (_) => handler((T)_));
            }
        }

        public void Register<T>(IMessageHandler<T> handler)
            where T : AsynchronousMessage
        {
            if (!_handlers.ContainsKey(typeof(T)))
            {
                _handlers.Add(typeof(T), (_) => handler.HandleMessage((T)_));
            }
        }

        public void Route(object msg)
        {
            var msgType = msg.GetType();

            if (_handlers.ContainsKey(msgType))
            {
                try
                {
                    _handlers[msgType](msg);
                }
                catch (Exception ex) //Catch all without logging or anything?
                {
                }
            }
        }
    }
}
