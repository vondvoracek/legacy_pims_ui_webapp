using DalSoft.RestClient.DependencyInjection;
using MI.PIMS.UI.Handlers;
using MI.PIMS.UI.Providers;
using MI.PIMS.UI.Services.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;


namespace MI.PIMS.UI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration _config) 
        {

            /*==========================
                // Add Kendo UI services to the services container
            ============================*/
            services.AddKendo();


            /*==========================
               Enable runtime compilation
            ============================*/
            // Pass down group policy to be implemented in options, such that every endpoint will validate the request
            services.AddRazorPages();//.AddMvcOptions(options => options.Filters.Add(new AuthorizeFilter("AccessGlobalGroupPolicy")));
;

            services.AddHttpContextAccessor();

          

            /*==========================
                Add framework services.
            ============================*/
            var mvcBuilder = services.AddControllersWithViews();
            #if DEBUG
                mvcBuilder.AddRazorRuntimeCompilation(); //It will allow to edit while application is running and then refresh the page to see changes
            #endif

            // Maintain property names during serialization. See:
            // https://github.com/aspnet/Announcements/issues/194
            mvcBuilder.AddNewtonsoftJson(options =>
                 {
                     options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                     options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.None;
                     options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;
                 });

            //services
            //    .AddControllersWithViews()                                
            //     Maintain property names during serialization. See:
            //     https://github.com/aspnet/Announcements/issues/194
            //    .AddNewtonsoftJson(options =>
            //    {
            //        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.None;
            //        options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;
            //    });

            services.AddMvc(config => {
                var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddRazorOptions(options =>
            {
                options.ViewLocationFormats.Add("/{0}.cshtml");                
            })
            .AddViewComponentsAsServices();

            services.AddRestClient("MIServicesAPI", _config.GetSection("AppSettings:MIServicesAPIUrl").Value);
            //services.AddRestClient("ServiceAPI", _config.GetSection("AppSettings:ServiceUrl").Value);

            /*==========================
                Get HttpClient for Base Service (Named Client)
            ============================*/
            services.AddHttpClient("HttpClient", c =>
            {
                c.BaseAddress = new Uri(_config.GetSection("AppSettings").GetSection("ServiceUrl").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler()
                {
                    UseDefaultCredentials = true
                };
            });

            /*==========================
                Get HttpClient for MI Services API (Named Client)
            ============================*/
            services.AddHttpClient("HttpClientMIServicesAPI", c =>
            {
                c.BaseAddress = new Uri(_config.GetSection("AppSettings").GetSection("MIServicesAPIUrl").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler()
                {
                    UseDefaultCredentials = true
                };
            });

            /*==========================
                Get Access Global Group
            ============================*/
            //services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

            //services.AddAuthorization(options =>
            //{
            //    //options.AddPolicy("AccessGlobalGroup", policy => policy.RequireRole(_config.GetSection("AppSettings").GetSection("AccessGlobalGroup").Value));
            //});

            //services.AddAuthentication(option =>
            //{
            //    option.DefaultAuthenticateScheme = "UserValidationScheme";

            //    option.DefaultForbidScheme = "UserValidationForbitScheme";

            //    option.AddScheme<UserAuthenticationHandler>("UserValidationForbitScheme", "UserValidationForbitScheme");
            //    option.AddScheme<UserAuthenticationHandler>("UserValidationScheme", "UserValidationScheme");
            //});

            //Comment this out for AD SSO Integration 

            //services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

            ////Comment this out for AD SSO Integration 
            ////services.AddAuthentication(IISDefaults.AuthenticationScheme);
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AccessGlobalGroupPolicy", policy =>
            //        policy.Requirements.Add(new AccessGlobalGroupRequirement()));
            //});

            //services.AddSingleton<IAuthorizationHandler, AccessGlobalGroupAuthorizationHandler>();

            services.AddAuthorization(options =>
            {
                foreach(Roles role in Enum.GetValues(typeof(Roles)))
                {
                    options.AddPolicy(role.ToString(), policy => policy.RequireRole(((int)role).ToString()));
                } 
            });

            // *** DO NOT REMOVE ***
            // Removed but keep here for future reference 
            services.AddRoleAuthorization<RoleProvider>();
            // *** DO NO REMOVE ENDS *** 

            return services;
        }
    }
}
