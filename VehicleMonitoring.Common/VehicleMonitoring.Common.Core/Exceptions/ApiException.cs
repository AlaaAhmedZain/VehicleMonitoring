using System;
using System.Collections.Generic;

namespace VehicleMonitoring.Common.Core.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }

        public List<string> Errors { get; set; }

        public ApiException(string message,
                            int statusCode = 500,
                            List<string> errors = null) :
            base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
        public ApiException(Exception ex, int statusCode = 500) : base(ex.Message)
        {
            StatusCode = statusCode;
        }
    }

    public class ApiError
    {
        public string message { get; set; }
        public bool isError { get; set; }
        public string detail { get; set; }
        public List<string> Errors { get; set; }

        public ApiError(string message)
        {
            this.message = message;
            isError = true;
        }

    }
}
