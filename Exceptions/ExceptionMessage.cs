using BaseTypes;
using System;

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
                Logger.Error($"Exception in {propName} \n\rException type: {exceptionType}. \n\rMessage: {errException.Message}" +
                              $"\n\rInnertMessage: {errException.InnerException.Message}");
            }
            else
            {
                Logger.Error($"Exception in {propName} \n\rException type: {exceptionType}. \n\rMessage: {errException.Message}");
            }
        }
    }
}
