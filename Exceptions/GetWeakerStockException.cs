﻿using System;
using System.Runtime.CompilerServices;

namespace Exceptions
{
    public class GetWeakerStockException : Exception
    {
        public GetWeakerStockException()
        {
            
        }
        
        public GetWeakerStockException(Exception errException, [CallerMemberName]string propName = "") : base(errException.Message)
        {
            var exceptionMessage = new ExceptionMessage(typeof(GetWeakerStockException), errException, propName);
        }
    }
}