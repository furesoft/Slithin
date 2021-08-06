using System;
using System.Collections.Generic;

namespace Slithin.Core
{
    public static class MessageRouter
    {
        private static Dictionary<Type, Action<object>> _handlers = new();

        public static void Register<T>(Action<T> handler)
            where T : AsynchronousMessage
        {
            _handlers.Add(typeof(T), (_) => handler((T)_));
        }

        public static void Route(object msg)
        {
            var msgType = msg.GetType();

            if (_handlers.ContainsKey(msgType))
            {
                try
                {
                    _handlers[msgType](msg);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
