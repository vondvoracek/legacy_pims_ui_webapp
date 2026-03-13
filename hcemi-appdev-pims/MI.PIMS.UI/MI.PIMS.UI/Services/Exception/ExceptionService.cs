using MI.PIMS.UI.Common;
using MI.PIMS.UI.Services.Email;
using MI.PIMS.UI.Services.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using System;
using System.Net;

namespace MI.PIMS.UI.Services
{
    public static class ExceptionService
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerService logger, IEmailService emailService, ErrorType errorType, Helper helper)
        {            
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {            
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        string traceIdentifierId = context.Request.HttpContext.TraceIdentifier;

                        logger.Error($"Something went wrong: {contextFeature.Error}");
                        logger.Debug(helper.CloudMailServiceAuthTokenUrl + " " + helper.CloudMailServiceClientId + " " + helper.CloudMailServiceClientSecret);
                        await emailService.SendExceptionEmailAsync(contextFeature.Error.ToString() + "<br/><br/>Reference: " + traceIdentifierId.ToStringNullSafe());
                        //context.Response.AppApplicationError(contextFeature.Error.Message);

                        context.Response.Redirect((String.IsNullOrEmpty(helper.VirtualDirectory) ? "" : "/" + helper.VirtualDirectory) + "/ErrorHandler/Index?id=" + context.Response.StatusCode, true);
                    }                    
                });                                 
            });
            //app.UseExceptionHandler("/ErrorHandler/Index");
            //app.UseStatusCodePagesWithRedirects("/ErrorHandler/Index?id={0}");
        }
    }
}
