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

        public BotOnMessageException(Exception errException, [CallerMemberName]string propName = "") : base(errException.Message)
        {
            var exceptionMessage = new ExceptionMessage(typeof(BotOnMessageException), errException, propName);
        }
    }
}
