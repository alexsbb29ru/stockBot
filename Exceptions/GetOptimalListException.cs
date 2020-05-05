using System;
using System.Runtime.CompilerServices;

namespace Exceptions
{
    public class OptimalListException : Exception
    {
        public OptimalListException()
        {

        }

        public OptimalListException(Exception errException, [CallerMemberName]string propName = "") : base(errException.Message)
        {
            var unused = new ExceptionMessage(typeof(OptimalListException), errException, propName);
        }
    }
}
