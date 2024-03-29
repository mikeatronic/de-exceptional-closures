using de_exceptional_closures.Config;
using de_exceptional_closures.Notify;
using de_exceptional_closures_infraStructure.Behaviours;
using de_exceptional_closures_infraStructure.Data;
using de_exceptional_closures_infraStructure.Features.ReasonType.Validation;
using de_exceptional_closures_Infrastructure.Data;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using static de_exceptional_closures_infraStructure.Behaviours.ExceptionBehaviour;

namespace de_exceptional_closures
{
    public class Startup
    {
        protected readonly string currentEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            currentEnvironment = env.EnvironmentName;
        }

        public IConfiguration Configuration { get; }
        protected CloudFoundryServicesOptions CloudFoundryServicesOptions;
        protected NotifyConfig NotifyConfig = new NotifyConfig();
        protected SchoolsApiConfig SchoolsApiConfig = new SchoolsApiConfig();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GetAllReasonTypesQueryValidator>());
            services.AddSingleton(services);
            services.AddOptions();
            services.ConfigureCloudFoundryOptions(Configuration);
            CloudFoundryServicesOptions = Configuration.GetSection("vcap").Get<CloudFoundryServicesOptions>();

            // Setup mysql database
            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Configuration));

            // Setup identity
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Nlog
            if (currentEnvironment == "Development")
            {
                // Set Nlog Connection string
                GlobalDiagnosticsContext.Set("DefaultNlogConnection", ConfigurationFactory.PopulateLocalConnectionString(Configuration));
            }
            else
            {
                MySqlCredentials adminApplication;

                adminApplication = ConfigurationFactory.CreateDatabaseConfig(CloudFoundryServicesOptions
               .Services["mysql"].First(s => s.Name == "de-exceptional-closures-mysql").Credentials);

                // Set Nlog Connection string
                GlobalDiagnosticsContext.Set("DefaultNlogConnection", ConfigurationFactory.PopulateConnectionString(adminApplication));
            }

            if (currentEnvironment == "Development" && bool.Parse(Configuration["RequiresProxy"]))
            {
                    // Needed to bypass proxy
                    HttpClient.DefaultProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
 
            // Notify Config
            NotifyConfig = ConfigurationFactory.CreateNotifyConfig(CloudFoundryServicesOptions
           .Services["user-provided"]
           .First(s => s.Name == "de-exceptional-closures-notify").Credentials["Credentials"]);

            services.Configure<NotifyConfig>(nc => nc.PopulateNotifyConfig(NotifyConfig));

            services.AddTransient<INotifyService, NotifyService>();

            // Schools api config
            SchoolsApiConfig = ConfigurationFactory.CreateSchoolsApiConfig(CloudFoundryServicesOptions
           .Services["user-provided"]
           .First(s => s.Name == "de-institution-api-secrets").Credentials["Credentials"]);

            services.Configure<SchoolsApiConfig>(s => s.PopulateSchoolsApiConfig(SchoolsApiConfig));

            services.AddHttpClient("InstitutionsClient", p => p.BaseAddress = new Uri(SchoolsApiConfig.ApiUrl));

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.Name = "DEClosuresCookie";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            // Third party libraries
            services.AddAutoMapper(typeof(Startup).GetTypeInfo().Assembly, typeof(ApplicationDbContext).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly, typeof(ApplicationDbContext).GetTypeInfo().Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Added for Zap recommendation - X-Frame-Options Header Not Set
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-XSS-Protection", "1; mode = block");

                context.Response.GetTypedHeaders().CacheControl =
                           new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                           {
                               NoCache = true,
                               NoStore = true,
                           };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Pragma] = new string[] { "no-cache" };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] = new string[] { "no-store, no-cache, must-revalidate, max-age=0" };

                await next();
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var deExcelClosure = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                deExcelClosure.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
