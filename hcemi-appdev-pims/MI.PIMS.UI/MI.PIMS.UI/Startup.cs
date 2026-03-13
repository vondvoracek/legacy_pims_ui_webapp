using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MI.PIMS.UI.Extensions;
using MI.PIMS.UI.Services.Logging;
using MI.PIMS.UI.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MI.PIMS.UI.Repositories;
using MI.PIMS.UI.Services;
using Microsoft.AspNetCore.HttpOverrides;
using MI.PIMS.UI.Services.Email;
using MI.PIMS.UI.Services.Maintenance;
using MI.PIMS.UI.Services.MIAuthenticate;
using MI.PIMS.UI.Areas.EPAL.Repositories;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BL.Services;
using MI.PIMS.BL.Repositories;
using MI.PIMS.BL;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.Security.Claims;
using System.Threading.Tasks;
using StackExchange.Redis;
using MI.PIMS.UI.Middlewares;
using MI.PIMS.UI.Providers;
using System.Linq;
using MI.PIMS.UI.Services.PimsStaticDataRepositories;
using MI.PIMS.BL.Common.Telemetry;
using Microsoft.ApplicationInsights.DependencyCollector;
using MI.PIMS.UI.Areas.DPOC.Repositories;
using MI.PIMS.BO.Services;
using System.Security.AccessControl;

using OpenTelemetry.Trace;
using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry;
using OpenTelemetry.Resources;
using MI.PIMS.BL.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace MI.PIMS.UI
{
    public class Startup
    {
        public IConfiguration _config { get; }
        public static IConfiguration StaticConfig { get; private set; }
        public static bool IsAppLoaded { get; set; }

        public Startup(IConfiguration config)
        {
            _config = config;
            StaticConfig = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddNLog();
            });

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(_config.GetSection("AzureAd"));


            services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Events.OnRedirectToIdentityProvider += OnRedirectToIdentityProviderFunc;
                options.Events.OnRedirectToIdentityProviderForSignOut += OnRedirectToIdentityProviderForSignOutFunc;
            });

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });


            services.AddRazorPages()
                .AddMicrosoftIdentityUI();

            services.AddApplicationServices(_config);

            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = true;
            });

            services.AddApplicationInsightsTelemetry(options =>
            {
                options.ConnectionString = _config.GetSection("ApplicationInsights:ConnectionString")?.Value.ToString();
                options.EnableAdaptiveSampling = false;
                options.EnableDependencyTrackingTelemetryModule = true;
            });

            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
            {
                module.EnableSqlCommandTextInstrumentation = true;
            });

            services.AddOpenTelemetry()
                .WithTracing(traceProviderBuilder =>
                {
                    traceProviderBuilder
                        .SetResourceBuilder(
                            ResourceBuilder.CreateDefault()
                                .AddService(serviceName: "PIMS-UI", serviceVersion: "1.0.0")
                        )
                        .SetSampler(new AlwaysOnSampler())
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSource("Npgsql")
                        .AddAzureMonitorTraceExporter(o =>
                        {
                            o.ConnectionString = _config.GetSection("ApplicationInsights:ConnectionString")?.Value.ToString();
                        });
                });


            // Register DbContext with PostgreSQL
            services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(_config.GetSection("AppSettings:PostgresConnectionStrings").Value));

            ////Add types here for dependency injections                        
            //services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IWebHostEnvironment, IWebHostEnvironment>();
            services.AddSingleton(typeof(TelemetryLogger<>));
            services.AddTransient<ClaimsPrincipal, ClaimsPrincipal>();

            //services.AddLogging();
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddSingleton<BL.Common.ILoggerService, BL.Common.LoggerService>();

            //DYU 12/18/2024 - Services for Microsoft Graph;
            //services.AddSingleton<IGraphHandlerService, GraphHandlerService>();

            if (_config.GetSection("AppSettings:CacheType").Value == "Memcached")
            {
                /*Memcached configuration and injection*/
                services.AddEnyimMemcached(_config);
                services.AddSingleton<ICacheProvider, CacheProvider>();
                services.AddSingleton<ICacheRepository, CacheRepository>();
                /*End Memcached*/
            }
            else
            {
                //Redis cache
                services.AddSingleton<ICacheProvider, RedisCacheProvider>();
                services.AddSingleton<ICacheRepository, RedisCacheRepository>();
                //End Redis cache            
            }


            /* EIS Security Configuration Settings*/
            services.Configure<Models.Config.EISSecurityModel>(_config.GetSection("EISSecurity"));
            /* AppSettings Configuration Settings*/
            services.Configure<Models.Config.AppSettings>(_config.GetSection("AppSettings"));
            /* SmtpSettings Configuration Settings*/
            services.Configure<Models.Config.SmtpSettings>(_config.GetSection("SmtpSettings"));
            //AzureMailServiceSettings
            services.Configure<Models.Config.AzureMailServiceSettings>(_config.GetSection("AzureMailServiceSettings"));

            // AD and Authentication
            services.AddSingleton<Services.ActiveDirectory.IActiveDirectoryService, Services.ActiveDirectory.ActiveDirectoryService>();
            services.AddSingleton<IMIAuthenticateService, MIAuthenticateService>();
            services.AddSingleton<BL.Services.Interfaces.IActiveDirectoryService, BL.Services.ActiveDirectoryService>();

            services.AddTransient<HomeRepository, HomeRepository>();
            services.AddTransient<MarketRepository, MarketRepository>();
            services.AddTransient<AppDataRoleRepository, AppDataRoleRepository>();
            services.AddTransient<IUserAccessHistRepository, UserAccessHistRepository>();
            //services.AddTransient<IEmailService, EmailService>(); /*Commented out the on-prem mail service*/
            services.AddTransient<IEmailService, CloudeEmailService>();
            services.AddTransient<IEISSecurityService, EISSecurityService>();
            services.AddTransient<IMaintenanceService, MaintenanceService>();
            services.AddTransient<ICnSIFPLogic, CnSIFPLogic>();

            services.AddSingleton<Helper, Helper>();
            services.AddScoped<GenericGlobal, GenericGlobal>();
            services.AddTransient<ErrorType, ErrorType>();
            services.AddScoped<IStaticDataCachingService, StaticDataCachingService>();
            services.AddSingleton<MIUrlHelper, MIUrlHelper>();

            /**********************************************************************************************************
             * ALL API SERVICES/REPOS REGISTRATION
             **********************************************************************************************************/
            services.AddSingleton<BL.Common.Helper, BL.Common.Helper>();
            services.AddScoped<IMenuAccessService, MenuAccessService>();
            services.AddScoped<IAppRoleService, AppRoleService>();
            services.AddScoped<IEPALProceduresService, EPALProceduresService>();
            services.AddScoped<IPIMSValidValuesService, PIMSValidValuesService>();
            services.AddScoped<IXrefStateService, XrefStateService>();
            services.AddScoped<IXrefStatusService, XrefStatusService>();
            services.AddSingleton<IUserInfoService, UserInfoService>();
            services.AddScoped<IEntityXrefService, EntityXrefService>();
            services.AddScoped<IRefDiagnosesService, RefDiagnosesService>();
            services.AddScoped<IRefRevenuesService, RefRevenuesService>();
            services.AddScoped<IRefModifiersService, RefModifiersService>();
            services.AddScoped<IUserAccessHistService, UserAccessHistService>();
            services.AddScoped<IRefProceduresService, RefProceduresService>();
            services.AddScoped<IDPOCService, DPOCService>();
            services.AddScoped<IReportsService, ReportsService>();
            services.AddScoped<ICategoryUpdateService, CategoryUpdateService>();
            services.AddScoped<IKLPoliciesService, KLPoliciesService>();
            services.AddScoped<IDPOCGuidelineDTQsService, DPOCGuidelineDTQsService>();
            services.AddSingleton<UserInfoRepository, UserInfoRepository>();
            services.AddScoped<MenuAccessRepository, MenuAccessRepository>();
            services.AddScoped<AppRoleRepository, AppRoleRepository>();
            services.AddScoped<XrefStateRepository, XrefStateRepository>();
            services.AddScoped<XrefStatusRepository, XrefStatusRepository>();
            services.AddScoped<EPALProceduresRepository, EPALProceduresRepository>();
            services.AddScoped<PIMSValidValuesRepository, PIMSValidValuesRepository>();
            services.AddScoped<RefDiagnosesRepository, RefDiagnosesRepository>();
            services.AddScoped<RefRevenuesRepository, RefRevenuesRepository>();
            services.AddScoped<RefModifiersRepository, RefModifiersRepository>();
            services.AddScoped<RefProceduresRepository, RefProceduresRepository>();
            services.AddScoped<ReportsRepository, ReportsRepository>();
            services.AddScoped<CategoryUpdateRepository, CategoryUpdateRepository>();

            services.AddScoped<TestOracleRepository, TestOracleRepository>();
            services.AddScoped<Entity_XrefRepository, Entity_XrefRepository>();

            services.AddScoped<IPayCodeProceduresService, PayCodeProceduresService>();
            services.AddScoped<BL.Repositories.PayCodeProceduresRepository, BL.Repositories.PayCodeProceduresRepository>();

            /* DPOC */
            services.AddScoped<IDPOCService, DPOCService>();
            services.AddScoped<BL.Repositories.DPOCRepository, BL.Repositories.DPOCRepository>();

            services.AddScoped<DPOCRepository, DPOCRepository>();

            services.AddScoped<DPOCGuidelineDTQsRepository, DPOCGuidelineDTQsRepository>(); // Guideline Configuration utlitizes it

            services.AddScoped<IDPOCGuidelineRulesService, DPOCGuidelineRulesService>();
            services.AddScoped<BL.Repositories.DPOCGuidelineRulesRepository, BL.Repositories.DPOCGuidelineRulesRepository>();

            services.AddScoped<IDPOCGuidelineDiagnosisCodesService, DPOCGuidelineDiagnosisCodesService>();
            services.AddScoped<BL.DPOCGuidelineDiagnosisCodesRepository, BL.DPOCGuidelineDiagnosisCodesRepository>();

            services.AddScoped<IDPOCGuidelineStatesService, DPOCGuidelineStatesService>();
            services.AddScoped<BL.Repositories.DPOCGuidelineStatesRepository, BL.Repositories.DPOCGuidelineStatesRepository>();

            services.AddScoped<IDPOCGuidelinePOSService, DPOCGuidelinePOSService>();
            services.AddScoped<DPOCGuidelinePOSRepository, DPOCGuidelinePOSRepository>();

            services.AddScoped<KLPoliciesRepository, KLPoliciesRepository>();

            services.AddScoped<PimsValidValuesManager, PimsValidValuesManager>();
            services.AddScoped<PaycHierarchyCodesXWalkManager, PaycHierarchyCodesXWalkManager>();

            services.AddScoped<DPOCHierarchyCodesXWalkUIRepository, DPOCHierarchyCodesXWalkUIRepository>();

            services.AddScoped<DPOCHierarchyCodesXWalkRepository, DPOCHierarchyCodesXWalkRepository>();
            services.AddScoped<IDPOCHierarchyCodesXWalkService, DPOCHierarchyCodesXWalkService>();

            services.AddScoped<IEPAL_Altrnt_Svc_CatService, EPALAltrnt_Svc_CatService>();
            services.AddScoped<EPALAltrnt_Svc_CatRepository, EPALAltrnt_Svc_CatRepository>();


            services.AddScoped<ManageGuidelinesRepository>();
            services.AddScoped<IManageGuidelinesService, ManageGuidelinesService>();

            services.AddScoped<IManualGuidelineRepository, ManualGuidelineRepository>();
            services.AddScoped<IManualGuidelineService, ManualGuidelineService>();

            /* The relevant part for Forwarded Headers */
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        private async Task OnRedirectToIdentityProviderForSignOutFunc(RedirectContext context)
        {
            if (_config["AppSettings:AppBaseUrl"] != null)
            {
                context.ProtocolMessage.PostLogoutRedirectUri = $"{_config["AppSettings:AppBaseUrl"].TrimEnd(Convert.ToChar("/"))}{_config["AzureAd:SignedOutCallbackPath"]}";
            }

            await Task.CompletedTask;
        }

        private async Task OnRedirectToIdentityProviderFunc(RedirectContext context)
        {
            if (_config["AppSettings:AppBaseUrl"] != null)
            {
                context.ProtocolMessage.RedirectUri = $"{_config["AppSettings:AppBaseUrl"].TrimEnd(Convert.ToChar("/"))}{_config["AzureAd:CallbackPath"]}";
            }

            await Task.CompletedTask;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerService logger, IStaticDataCachingService staticDataCachingService, IEmailService emailService,
            IEISSecurityService _eISSecurityService, ErrorType errorType, Helper helper)
        {
            if (env.IsEnvironment("Local"))
            {
                //app.UseDeveloperExceptionPage();
                //Global Exception Handler                
                app.ConfigureExceptionHandler(logger, emailService, errorType, helper);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                //Global Exception Handler                
                app.ConfigureExceptionHandler(logger, emailService, errorType, helper);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 403)
                {
                    var refNo = context.HttpContext.Response.Headers.Where(x => x.Key == "refNo").FirstOrDefault().Value;
                    context.HttpContext.Response.Redirect((String.IsNullOrEmpty(helper.VirtualDirectory) ? "" : "/" + helper.VirtualDirectory) + "/ErrorHandler/Forbidden?refNo=" + refNo);
                }
                else if (context.HttpContext.Response.StatusCode == 404)
                {
                    context.HttpContext.Response.Redirect((String.IsNullOrEmpty(helper.VirtualDirectory) ? "" : "/" + helper.VirtualDirectory) + "/ErrorHandler/PageNotFound");
                }
                else if (context.HttpContext.Response.StatusCode == 500)
                {
                    context.HttpContext.Response.Redirect((String.IsNullOrEmpty(helper.VirtualDirectory) ? "" : "/" + helper.VirtualDirectory) + "/ErrorHandler/InternalServerError");
                }
            });

            app.UseForwardedHeaders();

            app.UseHttpContext();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            // Middleware declaration will go after authentication and authorization due to permission assignment first
            app.UseMiddleware<PermissionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");



                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Setting up static data into Cache -- static caching turned off.
            //staticDataCachingService.Set();
        }
    }
}
