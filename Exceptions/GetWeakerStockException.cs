using System;
using System.Runtime.CompilerServices;

namespace Exceptions
{
    public class WeakerStockException : Exception
    {
        public WeakerStockException()
        {
            
        }
        
        public WeakerStockException(Exception errException, [CallerMemberName]string propName = "") : base(errException.Message)
        {
            var unused = new ExceptionMessage(typeof(WeakerStockException), errException, propName);
        }
    }
}