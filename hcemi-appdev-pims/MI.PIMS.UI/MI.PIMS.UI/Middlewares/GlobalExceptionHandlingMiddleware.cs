using MI.PIMS.UI.Common;
using MI.PIMS.UI.Services.Email;
using MI.PIMS.UI.Services.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILoggerService _logger;
        private readonly IEmailService _emailService;
        private readonly Helper _helper;

        public GlobalExceptionHandlingMiddleware(
            ILoggerService loggerService,
            IEmailService emailService,
            Helper helper)
        {
            _logger = loggerService;
            _emailService = emailService;
            _helper = helper;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (System.Exception e)
            {
                _logger.Error(e.Message, e);

                await _emailService.SendExceptionEmailAsync(e.Message);

                //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                context.Response.Redirect((String.IsNullOrEmpty(_helper.VirtualDirectory) ? "" : "/" + _helper.VirtualDirectory) + "/ErrorHandler/Index?id=" + context.Response.StatusCode);
            }
        }
    }
}
