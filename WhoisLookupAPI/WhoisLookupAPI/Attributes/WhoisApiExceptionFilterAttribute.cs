namespace WhoisLookupAPI.Attributes
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc;
    using WhoisLookupAPI.Exceptions;
    using WhoisLookupAPI.Models.Response;
    using WhoisLookupAPI.Enumerations;
    using System;

    public class WhoisApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            WhoisErrorResponse errorResponse = new WhoisErrorResponse();

            // Handle specific exceptions
            if (context.Exception is ArgumentException argEx)
            {
                errorResponse.ErrorCode = ErrorCode.InvalidRequest;
                errorResponse.ErrorMessage = argEx.Message;
                context.Result = new BadRequestObjectResult(errorResponse);
            }
            else if (context.Exception is InvalidRequestDataException invalidReqEx)
            {
                errorResponse.ErrorCode = ErrorCode.InvalidRequest;
                errorResponse.ErrorMessage = invalidReqEx.Message;
                context.Result = new BadRequestObjectResult(errorResponse);
            }
            else if (context.Exception is WhoisApiException whoisApiEx)
            {
                errorResponse.ErrorCode = ErrorCode.ExternalApiError;
                errorResponse.ErrorMessage = whoisApiEx.Message;
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = (int)whoisApiEx.StatusCode
                };
            }
            else if (context.Exception is UnauthorizedAccessException unauthorizedEx)
            {
                errorResponse.ErrorCode = ErrorCode.UnauthorizedAccess;
                errorResponse.ErrorMessage = unauthorizedEx.Message;
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = 401 // Unauthorized
                };
            }
            else if (context.Exception is InvalidOperationException invalidOpEx)
            {
                errorResponse.ErrorCode = ErrorCode.InternalServerError;
                errorResponse.ErrorMessage = invalidOpEx.Message;
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = 500 // Internal Server Error
                };
            }
            else
            {
                // Handle any unexpected errors
                errorResponse.ErrorCode = ErrorCode.Unknown;
                errorResponse.ErrorMessage = "An unknown error occurred.";
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = 500 // Internal Server Error
                };
            }

            context.ExceptionHandled = true;
        }
    }
}
