using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Authen;
using WebAPI.BAL;
using WebAPI.Model;
using WebAPI.Repo;

namespace Code_Test
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<AppSetting>(Configuration.GetSection("AppSetting"));

            services.AddDbContext<WebAPIDBContext>(opts => opts.UseSqlServer(Configuration["AppSetting:ConnectionString"]));

            services.AddCors(options =>
            {
                options.AddPolicy("MyAllowSpecificOrigins",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration["AppSetting:ConnectionString"]));
            services.AddHangfireServer();

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(0, 0);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["AppSetting:Issuer"],
                    ValidAudience = Configuration["AppSetting:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["AppSetting:Secret"]))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthorizationHeaderRequirement", policy =>
                    policy.Requirements.Add(new AuthorizationHeaderRequirement()));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0.0", new OpenApiInfo { Title = "CodeTest API", Version = "v0.0" });
                c.OperationFilter<RemoveVersionFromParameter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPath>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                        new string[] { }
                    }
                });
            });

            services.AddSingleton<IAuthorizationHandler, AuthorizationHeaderHandler>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            services.AddScoped<IEVoucherRepo, EVoucherRepo>();
            services.AddScoped<IPaymentRepo, PaymentRepo>();
            services.AddScoped<IPurchaseRepo, PurchaseRepo>();
            services.AddScoped<ITranscationRepo, TranscationRepo>();

            services.AddScoped(typeof(IGenericBAL<>), typeof(GenericBAL<>));
            services.AddScoped<IEVoucherBAL, EVoucherBAL>();
            services.AddScoped<IPaymentBAL, PaymentBAL>();
            services.AddScoped<IPurchaseBAL, PurchaseBAL>();
            services.AddScoped<ITranscationBAL, TranscationBAL>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("MyAllowSpecificOrigins");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v0.0/swagger.json", "Versioned API v0.0");
                c.DocumentTitle = "API Documentation";
                c.DocExpansion(DocExpansion.List);
            });
        }

        public class RemoveVersionFromParameter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                try
                {
                    var versionParameter = operation.Parameters.Single(p => p.Name == "version");
                    operation.Parameters.Remove(versionParameter);
                }
                catch (Exception ex)
                {
                }
            }
        }
        public class ReplaceVersionWithExactValueInPath : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                try
                {
                    var paths = new OpenApiPaths();
                    foreach (var path in swaggerDoc.Paths)
                    {
                        paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);
                    }
                    swaggerDoc.Paths = paths;
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
