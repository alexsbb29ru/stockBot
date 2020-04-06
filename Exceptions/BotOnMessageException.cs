using System;
using System.Runtime.CompilerServices;

namespace Exceptions
{
    public class BotOnMessageException : Exception
    {
        public BotOnMessageException()
        {

        }

        public BotOnMessageException(Exception errException, [CallerMemberName]string propName = "") : base(errException.Message)
        {
            var unused = new ExceptionMessage(typeof(BotOnMessageException), errException, propName);
        }
    }
}
