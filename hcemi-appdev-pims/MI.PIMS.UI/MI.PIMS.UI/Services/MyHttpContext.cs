using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace MI.PIMS.UI.Services
{
    public class MyHttpContext
    {
        private static IHttpContextAccessor m_httpContextAccessor;

        public static HttpContext Current => m_httpContextAccessor.HttpContext;

        public static string AppBaseUrl => $"{Current.Request.Scheme}://{Current.Request.Host}{Current.Request.PathBase}";

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            m_httpContextAccessor = contextAccessor;
        }

        public static string GetUserId()
        {
            ClaimsPrincipal principal = new ClaimsPrincipal();
            var userIdentity = (ClaimsIdentity)principal.Identity;
            var nameId = userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            return nameId;
        }
    }

    public static class HttpContextExtensions
    {
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseHttpContext(this IApplicationBuilder app)
        {
            MyHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            return app;
        }


    }
}
