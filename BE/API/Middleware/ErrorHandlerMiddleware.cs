using Business.Communication;
using Business.CustomException;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public class ErrorHandlerMiddleware
{
    #region Property
    private readonly RequestDelegate _next;
    #endregion

    #region Constructor
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    #endregion

    #region Method
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var messageError = string.Empty;
            
            // Using switch for custom exception
            switch (error)
            {
                // Add custom exception code below!
                case MessageResultException ex:
                    messageError = ex.Message;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    // unhandled error
                    messageError = "Internal Server Error";
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Log.Error(error, messageError);

            var result = JsonSerializer.Serialize(new BaseResponse<object>(messageError));
            await response.WriteAsync(result);
        }
    }
    #endregion
}