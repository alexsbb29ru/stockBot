using System;
using System.Runtime.CompilerServices;

namespace Exceptions
{
    public class BotOnCallBackException : Exception
    {
        public BotOnCallBackException()
        {

        }

        public BotOnCallBackException(Exception errException, [CallerMemberName]string propName = "") : base(errException.Message)
        {
            var unused = new ExceptionMessage(typeof(BotOnCallBackException), errException, propName);
        }
    }
}
