using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Exceptions
{
    public class BotOnCallBackException : Exception
    {
        public BotOnCallBackException()
        {

        }

        public BotOnCallBackException(string message, [CallerMemberName]string propName = "") : base(message)
        {
            new ExceptionMessage(typeof(BotOnCallBackException), message, propName);
        }

        public BotOnCallBackException(string message, Exception inner, [CallerMemberName]string propName = "") : base(message, inner)
        {
            new ExceptionMessage(typeof(BotOnCallBackException), message, inner, propName);
        }
    }
}
