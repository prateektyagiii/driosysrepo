using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using OutReach.CoreAPI.Filters;
using OutReach.CoreSharedLib.Helpers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using OutReach.CoreAPI.Middlewares;
using OutReach.CoreService.Service;
using OutReach.CoreDataAccess.PostgreDBConnection;
using OutReach.CoreRepository.PGRepository;
using OutReach.CoreBusiness.Repository;
using OutReach.CoreSharedLib.Model.Common;
using OutReach.CoreSharedLib.Utilities;

namespace OutReach.CoreAPI
{
    public class Startup
    {
        ILogger<Startup> _logger;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            PgDbRepository.SetPgConnection(Configuration);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.AddHsts(options =>
            {
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddMvcCore().AddApiExplorer();
            services.AddMvc(option => option.EnableEndpointRouting = false).AddNewtonsoftJson();
            services.AddSignalR();
            services.AddControllers()
                                .AddNewtonsoftJson(options =>
                                {
                                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                                });

            services.AddApplicationInsightsTelemetry();
            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<AuthenticationService>();


            services.AddScoped<PgDbRepository>();
            services.AddScoped<AuthenticationRepository>();
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            var profileConfig = Configuration
             .GetSection("ProfileConfiguration")
             .Get<ProfileConfiguration>();
            services.AddSingleton(profileConfig);

            services.AddScoped<EmailService>();
            services.AddScoped<UploadImage>();
            services.AddScoped<UserRepository>();
            services.AddScoped<SchoolRepository>();
            services.AddScoped<InterestRepository>();
            services.AddScoped<EventRepository>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCors(options =>
            {
                var orgin = Configuration.GetSection("AllowOriginURL").Get<string[]>();
                options.AddPolicy("AllowOutReachASPNetCoreOrigin",
                builder => builder.WithOrigins(orgin).AllowAnyMethod().AllowAnyHeader());
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            
            services.AddScoped<AuthenticationFilter>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            

            #region "Swagger"  	    
            //Swagger API documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OutReach API", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                //In Test project find attached swagger.auth.pdf file with instructions how to run Swagger authentication 
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            });
            #endregion

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            _logger = logger;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHsts();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "App_Media")),
                RequestPath = new PathString("/App_Media")
            });
          
          
            app.UseCors("AllowOutReachASPNetCoreOrigin");
            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseRouting();
            //app.UseAntiXssMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();
            app.BearerAuthorization();
            //app.UseMiddleware<SerilogUseInfoMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
            //Swagger API documentation
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OutReach API");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}