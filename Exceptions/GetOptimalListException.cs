using System;
using System.Runtime.CompilerServices;

namespace Exceptions
{
    public class GetOptimalListException : Exception
    {
        public GetOptimalListException()
        {

        }

        public GetOptimalListException(Exception errException, [CallerMemberName]string propName = "") : base(errException.Message)
        {
            var unused = new ExceptionMessage(typeof(GetOptimalListException), errException, propName);
        }
    }
}
