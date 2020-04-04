﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Exceptions
{
    public class BotOnCallBackException : Exception
    {
        public BotOnCallBackException()
        {

        }

        public BotOnCallBackException(Exception errException, [CallerMemberName]string propName = "") : base(errException.Message)
        {
            var exceptionMessage = new ExceptionMessage(typeof(BotOnCallBackException), errException, propName);
        }
    }
}
