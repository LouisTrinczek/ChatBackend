using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Chat.Application.Exceptions
{
    public class InternalServerError : ObjectResult
    {
        private const int DefaultStatusCode = StatusCodes.Status500InternalServerError;

        public InternalServerError([ActionResultObjectValue] object? error)
            : base(error)
        {
            StatusCode = DefaultStatusCode;
        }
    }

    public class Forbidden : ObjectResult
    {
        private const int DefaultStatusCode = StatusCodes.Status403Forbidden;

        public Forbidden([ActionResultObjectValue] object? error)
            : base(error)
        {
            StatusCode = DefaultStatusCode;
        }
    }
}
