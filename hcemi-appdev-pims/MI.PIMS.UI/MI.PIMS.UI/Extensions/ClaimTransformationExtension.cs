using MI.PIMS.UI.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Extensions
{
    public static class ClaimTransformationExtension
    {
        #region Public Static Methods

        /// <summary>
        /// Activates simple role authorization for Windows authentication for the ASP.Net Core web site.
        /// </summary>
        /// <typeparam name="TRoleProvider">The <see cref="Type"/> of the <see cref="ISimpleRoleProvider"/> implementation that will provide user roles.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> onto which to register the services.</param>
        public static void AddRoleAuthorization<TRoleProvider>(this IServiceCollection services)
            where TRoleProvider : class, IRoleProvider
        {
            services.AddSingleton<IRoleProvider, TRoleProvider>();
            services.AddSingleton<IClaimsTransformation, ClaimsTransformer>();
        }

        #endregion
    }
}
