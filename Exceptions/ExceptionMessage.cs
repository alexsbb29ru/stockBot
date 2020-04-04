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
        public ExceptionMessage(Type exceptionType, Exception errException, string propName = "")
        {
            if (errException.InnerException != null)
            {
                _logger.Error($"Exception in {propName} \n\rException type: {exceptionType}. \n\rMessage: {errException.Message}" +
                              $"\n\rInnertMessage: {errException.InnerException.Message}");
            }
            else
            {
                _logger.Error($"Exception in {propName} \n\rException type: {exceptionType}. \n\rMessage: {errException.Message}");
            }
        }
    }
}
