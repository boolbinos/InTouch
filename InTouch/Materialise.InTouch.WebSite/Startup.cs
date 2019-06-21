using Hangfire;
using Materialise.InTouch.BLL;
using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.Providers;
using Materialise.InTouch.BLL.Services.PostConfiguration;
using Materialise.InTouch.BLL.Services.Storage;
using Materialise.InTouch.WebSite;
using Materialise.InTouch.WebSite.Authorize;
using Materialise.InTouch.WebSite.Extensions;
using Materialise.InTouch.WebSite.Model;
using Materialise.InTouch.WebSite.Services.EmailService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Net;
using Materialise.InTouch.BLL.Configs;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook.Models.Config;
using Microsoft.AspNetCore.Mvc;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models.Config;
using Materialise.InTouch.WebSite.Configuration;
using Materialise.InTouch.WebSite.Authorize.Avatar;

namespace Materialise_InTouch_WebSite
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Setup();
            services.SetupDB(Configuration.GetConnectionString("InTouchConnection"));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserContext, WebSiteUserContext>();
            services.AddTransient<IAvatarClient, AvatarClient>();

            services.AddOptions();
            services.Configure<FacebookPostProviderConfig>(Configuration.GetSection("FacebookPostProvider"));
            services.Configure<SharePointPostProviderConfig>(Configuration.GetSection("SharePointPostProvider"));
            services.Configure<StorageConfig>(Configuration.GetSection("Storage"));
            services.Configure<FullscreenBatchConfig>(Configuration.GetSection("Posts"));
            services.Configure<PostConfig>(Configuration.GetSection("PostDefaultValues"));
            services.Configure<LatestCommentsConfig>(Configuration.GetSection("LatestComments"));
            services.Configure<UrlsInfo>(Configuration.GetSection("UrlsInfo"));
            services.Configure<GoogleAnalyticsConfiguration>(
                Configuration.GetSection("GoogleAnalyticsConfiguration"));


            services.AddSingleton<IFileManager, FileManager>();
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddAzureAd(options => Configuration.Bind("AzureAd", options))
            .AddCookie();

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add(typeof(CheckUserAttribute));
                options.CacheProfiles.Add("CacheFiles",
                    new CacheProfile()
                    {
                        Duration = 60 * 60 * 24 * Convert.ToInt32(Configuration.GetSection("CachePeriod:CacheFilesInDays").Value)
                    });
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAllRole", policy =>
                    policy.Requirements.Add(new RolesRequirement(Consts.Roles.Moderator, Consts.Roles.User)));

                options.AddPolicy("RequireModeratorRole", policy =>
                     policy.Requirements.Add(new RolesRequirement(Consts.Roles.Moderator)));
            });
            services.AddTransient<IAuthorizationHandler, RolesHandler>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Materialise.InTouch API", Version = "v1" });
            });

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailNotificationSender, EmailNotificationService>();

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("InTouchConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            app.UseSession();

            loggerFactory.AddFile(Configuration.GetSection("Logging:PathFormat").Value,
                   retainedFileCountLimit: Convert.ToInt32(Configuration.GetSection("Logging:RetainedFileCountLimit").Value),
                   minimumLevel: (LogLevel)Enum.Parse(typeof(LogLevel), Configuration.GetSection("Logging:LogLevel:Default").Value));

            serviceProvider.GetService<DatabaseInitializer>().Initialize();

            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");
            //app.UseStatusCodePages();
            var options = new RewriteOptions()
                .AddRedirectToHttps();
            app.UseRewriter(options);

            GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(serviceProvider));
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            var interval = int.Parse(Configuration.GetSection("HangfireScheduler")["ImportIntervalInHours"]);
            RecurringJob.AddOrUpdate<IExternalPostService>("facebook", x => x.ImportPostsAsync("facebook"), Cron.HourInterval(interval));
            RecurringJob.AddOrUpdate<IExternalPostService>("sharepoint", x => x.ImportPostsAsync("sharepoint"), Cron.HourInterval(interval));
            app.UseStaticFiles();
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.Map("/getDefaultPostDuration", duration =>
            {
                duration.Run(async context =>
                {
                    if (context.User.Identity.IsAuthenticated)
                    {
                        context.Response.Headers.Add("duration", (Configuration.GetSection("PostDefaultValues")["DurationInSeconds"]));
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    }
                });
            });

            app.Map("/getDefaultPostEndDate", duration =>
            {
                duration.Run(async context =>
                {
                    if (context.User.Identity.IsAuthenticated)
                    {
                        context.Response.Headers.Add("enddateforAdmins", (Configuration.GetSection("PostDefaultValues")["DisplayPeriodInDaysForAdmins"]));
                        context.Response.Headers.Add("enddateforUsers", (Configuration.GetSection("PostDefaultValues")["DisplayPeriodInDaysForUsers"]));
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    }
                });
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

        }
    }
}
