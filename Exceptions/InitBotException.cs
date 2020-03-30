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

        public InitBotException(string message, [CallerMemberName]string propName = "") : base(message)
        {
            new ExceptionMessage(typeof(InitBotException), message, propName);
        }

        public InitBotException(string message, Exception inner, [CallerMemberName]string propName = "") : base(message, inner)
        {
            new ExceptionMessage(typeof(InitBotException), message, inner, propName);
        }
    }
}
