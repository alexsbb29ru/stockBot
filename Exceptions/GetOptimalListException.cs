using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Exceptions
{
    public class GetOptimalListException : Exception
    {
        public GetOptimalListException()
        {

        }

        public GetOptimalListException(string message, [CallerMemberName]string propName = "") : base(message)
        {
            new ExceptionMessage(typeof(GetOptimalListException), message, propName);
        }

        public GetOptimalListException(string message, Exception inner, [CallerMemberName]string propName = "") : base(message, inner)
        {
            new ExceptionMessage(typeof(GetOptimalListException), message, inner, propName);
        }
    }
}
