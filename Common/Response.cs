using System;
using System.Collections.Generic;
using System.Text;

 namespace Common
{
    public class Response
    {
        public Response()
        {

        }

        public Response(string message, bool success, string exceptionMessage, string stackTrace)
        {
            this.Message = message;
            this.Success = success;
            this.ExceptionMessage = exceptionMessage;
            this.StackTrace = stackTrace;
        }
        public string Message { get; set; }
        public bool Success { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
    }
}
