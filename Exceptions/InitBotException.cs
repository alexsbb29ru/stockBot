using BaseTypes;
using System;
using System.Runtime.CompilerServices;

namespace Exceptions
{
    public class InitBotException : Exception
    {
        public InitBotException()
        {

        }

        public InitBotException(Exception errException, [CallerMemberName]string propName = "") : base(errException.Message)
        {
            var exceptionMessage = new ExceptionMessage(typeof(InitBotException), errException, propName);
        }
    }
}
