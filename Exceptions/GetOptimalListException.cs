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

        public GetOptimalListException(Exception errException, [CallerMemberName]string propName = "") : base(errException.Message)
        {
            var exceptionMessage = new ExceptionMessage(typeof(GetOptimalListException), errException, propName);
        }
    }
}
