using BaseTypes;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Exceptions
{
    class ExceptionMessage : BaseController
    {
        public ExceptionMessage()
        {

        }

        public ExceptionMessage(Type errException, string message, string propName = "")
        {
            _logger.Error($"Exception in {propName} \n\rException type: {errException}. Message: {message}");
        }

        public ExceptionMessage(Type errException, string message, Exception inner, string propName = "")
        {
            _logger.Error($"Exception in {propName} \n\rException type: {errException}. \n\rMessage: {message}" +
                $"\n\rInnertMessage: {inner.Message}");
        }
    }
}
