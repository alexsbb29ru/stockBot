using System;
using System.Runtime.CompilerServices;

namespace Exceptions
{
    public class EvaluateSecException : Exception
    {
        public EvaluateSecException()
        {

        }

        public EvaluateSecException(Exception errException, [CallerMemberName]string propName = "") : base(errException.Message)
        {
            var unused = new ExceptionMessage(typeof(EvaluateSecException), errException, propName);
        }
    }
}