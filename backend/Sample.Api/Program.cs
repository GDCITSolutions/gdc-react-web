using BE.LocalAccountabilitySystem.Business.Email;
using BE.LocalAccountabilitySystem.Business.Email.Adapters;
using BE.LocalAccountabilitySystem.Business.Managers;
using BE.LocalAccountabilitySystem.Business.Services;
using BE.LocalAccountabilitySystem.Business.Spreadsheets;
using BE.LocalAccountabilitySystem.Schema;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Net.Http.Headers;

namespace BE.LocalAccountabilitySystem.Api
{
    public class Program
    {
        public static void AddServices(WebApplicationBuilder builder) 
        {
            // adapters
            builder.Services.AddScoped<IMailKitAdapter, MailKitAdapter>();

            // emailers
            builder.Services.AddScoped<IResetPasswordEmailer, ResetPasswordEmailer>();

            // managers
            builder.Services.AddScoped<IUserManager, UserManager>();

            // spreadsheets
            builder.Services.AddScoped<ICsvReaderAdapterFactory, CsvReaderAdapterFactory>();
            builder.Services.AddScoped<IXLWorkbookAdapterFactory, XLWorkbookAdapterFactory>();
            builder.Services.AddScoped<IUserSpreadsheetParser, UserSpreadsheetParser>();

            // services
            builder.Services.AddScoped<ILookupService, LookupService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();
        }

        public static void Main(string[] args)
        {
            string corsPolicy = "cors_policy";

            var builder = WebApplication.CreateBuilder(args);

            // Add database context
            builder.Services.AddDbContext<SampleContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Sample")));

            // add services
            AddServices(builder);

            // add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: corsPolicy,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000",
                                                         "https://sometestsite.org")
                                            .WithMethods("GET", "POST", "PUT", "DELETE")
                                            .WithHeaders(
                                                  HeaderNames.ContentType,
                                                  HeaderNames.Origin,
                                                  HeaderNames.XRequestedWith,
                                                  HeaderNames.Accept)
                                            .WithExposedHeaders(HeaderNames.SetCookie)
                                            .SetPreflightMaxAge(TimeSpan.FromSeconds(600))
                                            .AllowCredentials();
                                  });
            });

            // add controllers
            builder.Services.AddControllers();

            // cookie authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "sample_session";
                    options.Cookie.Domain = builder.Configuration["Domain"];
                    options.Cookie.Path = "/";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.Headers["Location"] = context.RedirectUri;
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
                });

            builder.Services.AddAuthorization();

            // swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpContextAccessor();

            /* uncomment this if you'd like to use azure insights
            // application insights
            builder.Logging.AddApplicationInsights(
                configureTelemetryConfiguration: (config) =>
                config.ConnectionString = builder.Configuration["ApplicationInsights:InstrumentationKey"],
                configureApplicationInsightsLoggerOptions: (options) => { }
            );

            builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("Sample", LogLevel.Trace);
            */

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors(corsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None
            });

            app.MapControllers();

            app.Run();
        }
    }
}