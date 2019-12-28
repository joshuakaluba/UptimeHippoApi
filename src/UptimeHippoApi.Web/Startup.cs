using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using UptimeHippoApi.Data.DataAccessLayer.Authentication;
using UptimeHippoApi.Data.DataAccessLayer.MonitorLogs;
using UptimeHippoApi.Data.DataAccessLayer.Monitors;
using UptimeHippoApi.Data.DataAccessLayer.PushNotificationTokens;
using UptimeHippoApi.Data.DataContext;
using UptimeHippoApi.Data.Initialization;
using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Services.Authentication;
using UptimeHippoApi.UptimeHandler.Services.Monitoring;
using UptimeHippoApi.Web.MiddleWare;
using TokenOptions = UptimeHippoApi.Data.Models.Authentication.TokenOptions;

namespace UptimeHippoApi.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UptimeHippoDataContext>();
            services.AddScoped<IPushNotificationTokensRepository, PushNotificationTokensRepository>();
            services.AddScoped<IUserValidatorService, UserValidatorService>();
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<IMonitorsRepository, MonitorsRepository>();
            services.AddScoped<IMonitorLogsRepository, MonitorLogsRepository>();
            services.AddScoped<IMonitoringService, MonitoringService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddCors(o => o.AddPolicy("UptimeHippoCorsPolicy", corsBuilder =>
            {
                corsBuilder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequiredLength = 6;
                option.Password.RequiredUniqueChars = 0;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<UptimeHippoDataContext>()
              .AddDefaultUI()
              .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["TokenOptions:Issuer"],
                        ValidAudience = Configuration["TokenOptions:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenOptions:Key"])),
                    };
                });

            services.AddAuthorization(options => options.AddPolicy("Trusted", policy => policy.RequireClaim("DefaultUserClaim", "DefaultUserAuthorization")));
            services.AddOptions();
            services.Configure<TokenOptions>(Configuration.GetSection("TokenOptions"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseDatabaseErrorPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }

                app.UseRouting();

                app.UseCors("UptimeHippoCorsPolicy");
                app.UseMiddleware<MaintainCorsHeadersMiddleware>();
                app.UseStaticFiles();
                app.UseAuthentication();

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });

                DataContextInitializer.UpdateContext(serviceProvider).Wait();
                DataContextInitializer.CreateUserRoles(serviceProvider).Wait();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}