using System;
using System.Runtime.CompilerServices;

namespace Exceptions
{
    public class GetWeakerStockException : Exception
    {
        public GetWeakerStockException()
        {
            
        }
        
        public GetWeakerStockException(string message, [CallerMemberName]string propName = "") : base(message)
        {
            var exceptionMessage = new ExceptionMessage(typeof(GetWeakerStockException), message, propName);
        }

        public GetWeakerStockException(string message, Exception inner, [CallerMemberName]string propName = "") : base(message, inner)
        {
            var exceptionMessage = new ExceptionMessage(typeof(GetWeakerStockException), message, inner, propName);
        }
    }
}