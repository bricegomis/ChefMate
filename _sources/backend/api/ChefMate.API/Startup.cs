using ChefMate.API.Models.Documents;
using ChefMate.API.Repositories;
using ChefMate.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;

namespace ChefMate.API;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddControllers()
        .AddJsonOptions(options =>
         {
             options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
             options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
         });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChefMate.API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Enter JWT Bearer token **_only_**",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
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
            c.OperationFilter<AuthorizeCheckOperationFilter>();

        });

        var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(Log.Logger));
        var datetimeProvider = new DateTimeService();

        var ravenDbServer = Configuration["RavenDB_Server"];
        var ravenDbName = Configuration["RavenDB_DbName"];
        var ravenDbCertBase64 = configuration["RavenDB_CertificateBase64"];
        var ravenDbCertPassword = configuration["RavenDB_CertificatePassword"];
        var certBytes = Convert.FromBase64String(ravenDbCertBase64);
        var ravenDbCertificate = new X509Certificate2(certBytes, ravenDbCertPassword);
        services.AddSingleton<IDocumentStore>(sp =>
        {
            var store = new DocumentStore
            {
                Urls = [ravenDbServer],
                Database = ravenDbName,
                Conventions =
                {
                    MaxNumberOfRequestsPerSession = 10
                },
                Certificate = ravenDbCertificate,
            };

            store.Conventions.FindCollectionName = type =>
            {
                if (type == typeof(ProductDocument))
                    return "Products";
                return DocumentConventions.DefaultGetCollectionName(type);
            };

            store.Initialize();
            return store;
        });
        services.AddScoped(sp =>
        {
            var store = sp.GetRequiredService<IDocumentStore>();
            return store.OpenAsyncSession();
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();

        var key = Encoding.UTF8.GetBytes(configuration["AuthSettings:JwtKey"]);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["AuthSettings:JwtIssuer"],
                ValidAudience = configuration["AuthSettings:JwtAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        services.AddAuthorization();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChefMate.API v1"));
        }

        //app.UseHttpsRedirection();

        app.UseCors();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize =
            context.MethodInfo.DeclaringType.GetCustomAttributes(true)
            .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>().Any()
            || context.MethodInfo.GetCustomAttributes(true)
            .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>().Any();

        if (!hasAuthorize) return;

        operation.Security ??= new List<OpenApiSecurityRequirement>();

        var scheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        };

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [scheme] = new List<string>()
        });
    }
}

