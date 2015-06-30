using System;

namespace Common.CoreTypeExtensions
{
    public static class ExceptionExtensions
    {
        public static string GetFullErrorMessage(this Exception exception, string message = null)
        {
            message = string.IsNullOrEmpty(message)
                ? exception.Message
                : message + " => " + exception.Message;

            if (exception.InnerException == null)
                return message;

            return GetFullErrorMessage(exception.InnerException, message);
        }
    }
}
