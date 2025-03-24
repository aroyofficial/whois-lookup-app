namespace WhoisLookupAPI.Attributes
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc;
    using WhoisLookupAPI.Exceptions;
    using WhoisLookupAPI.Models.Response;
    using WhoisLookupAPI.Enumerations;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Global exception filter to handle and format API errors consistently.
    /// </summary>
    public class WhoisApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Handles exceptions and formats the response accordingly.
        /// </summary>
        /// <param name="context">The exception context.</param>
        public override void OnException(ExceptionContext context)
        {
            WhoisErrorResponse errorResponse = new WhoisErrorResponse();
            HttpStatusCode statusCode;

            switch (context.Exception)
            {
                case ArgumentException argEx:
                    errorResponse.ErrorCode = ErrorCode.InvalidRequest;
                    errorResponse.ErrorMessage = argEx.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case InvalidRequestDataException invalidReqEx:
                    errorResponse.ErrorCode = ErrorCode.InvalidRequest;
                    errorResponse.ErrorMessage = invalidReqEx.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case WhoisApiException whoisApiEx:
                    errorResponse.ErrorCode = ErrorCode.ApiError;
                    errorResponse.ErrorMessage = whoisApiEx.Message;
                    statusCode = whoisApiEx.StatusCode;
                    break;

                case HttpRequestException httpEx:
                    errorResponse.ErrorCode = ErrorCode.NetworkError;
                    errorResponse.ErrorMessage = "An error occurred while communicating with an external service.";
                    statusCode = HttpStatusCode.ServiceUnavailable;
                    break;

                case TaskCanceledException taskEx:
                    errorResponse.ErrorCode = ErrorCode.Timeout;
                    errorResponse.ErrorMessage = "The request timed out.";
                    statusCode = HttpStatusCode.RequestTimeout;
                    break;

                default:
                    errorResponse.ErrorCode = ErrorCode.UnknownError;
                    errorResponse.ErrorMessage = "An unexpected error occurred. Please try again later.";
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = (int)statusCode
            };
        }
    }
}