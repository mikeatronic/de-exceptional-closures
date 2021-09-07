using de_exceptional_closures.Config;
using de_exceptional_closures.Notify;
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
using System.Reflection;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Session
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = "DeSessionCookie";
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

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

            // Notify Config
            NotifyConfig = ConfigurationFactory.CreateNotifyConfig(CloudFoundryServicesOptions
               .Services["user-provided"]
               .First(s => s.Name == "de-exceptional-closures-notify").Credentials["Credentials"]);

            services.Configure<NotifyConfig>(nc => nc.PopulateNotifyConfig(NotifyConfig));

            services.AddTransient<INotifyService, NotifyService>();

            services.AddHttpClient("InstitutionsClient", p => p.BaseAddress = new Uri("https://de-institutions-api-sandbox.london.cloudapps.digital/api/v1/Institution/"));


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.ConsentCookie.Name = "DECconsentCookie";
                options.CheckConsentNeeded = context => true;
                options.ConsentCookie.Expiration = TimeSpan.FromDays(90);
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
                options.Secure = CookieSecurePolicy.Always;
            });

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

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseSession();

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
