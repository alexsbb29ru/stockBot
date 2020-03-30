using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Exceptions
{
    public class BotOnMessageException : Exception
    {
        public BotOnMessageException()
        {

        }

        public BotOnMessageException(string message, [CallerMemberName]string propName = "") : base(message)
        {
            new ExceptionMessage(typeof(BotOnMessageException), message, propName);
        }

        public BotOnMessageException(string message, Exception inner, [CallerMemberName]string propName = "") : base(message, inner)
        {
            new ExceptionMessage(typeof(BotOnMessageException), message, inner, propName);
        }
    }
}
