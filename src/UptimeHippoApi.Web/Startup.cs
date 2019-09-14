using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UptimeHippoApi.Data.DataAccessLayer.Authentication;
using UptimeHippoApi.Data.DataAccessLayer.PushNotificationTokens;
using UptimeHippoApi.Data.DataContext;
using UptimeHippoApi.Data.Initialization;
using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Models.Static;
using UptimeHippoApi.Data.Services.Messaging;
using UptimeHippoApi.Web.MiddleWare;
using TokenOptions = UptimeHippoApi.Data.Models.Authentication.TokenOptions;

namespace UptimeHippoApi.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
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
            services.AddScoped<IMessagingService, MessagingService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddCors(o => o.AddPolicy("UptimeHippoCorsPolicy", corsBuilder =>
            {
                corsBuilder.WithOrigins("http://localhost:4200")
                    .WithOrigins("https://testzone.kaluba.tech")
                    .WithOrigins("https://*")
                    .WithOrigins("http://*")
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApplicationConfig.JwtTokenKey)),
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAuthorization(options => options.AddPolicy("Trusted", policy => policy.RequireClaim("DefaultUserClaim", "DefaultUserAuthorization")));
            services.AddOptions();
            services.Configure<TokenOptions>(Configuration.GetSection("TokenOptions"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseBrowserLink();
                    app.UseDatabaseErrorPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }

                app.UseCors("UptimeHippoCorsPolicy");
                app.UseMiddleware<MaintainCorsHeadersMiddleware>();
                app.UseStaticFiles();
                app.UseAuthentication();
                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });

                DataContextInitializer.Seed(app.ApplicationServices).Wait();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}