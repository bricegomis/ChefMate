using ChefMate.API.Extensions;
using ChefMate.API.Filters;
using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Documents.Interfaces;
using ChefMate.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using Raven.Client.Documents.Indexes;
using ChefMate.API.Indexes;

namespace ChefMate.API;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration ?? throw new ArgumentNullException(nameof(configuration));

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

        services.AddControllers(options =>
        {
            options.Filters.Add<UnescapeIdActionFilter>();
        })
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

        services.AddAttributedServices();

        RegisterRavenDb(services);

        var jwtKey = Configuration["AuthSettings:JwtKey"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT Key is not configured in AuthSettings:JwtKey.");
        }
        var key = Encoding.UTF8.GetBytes(jwtKey);
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
                ValidIssuer = Configuration["AuthSettings:JwtIssuer"],
                ValidAudience = Configuration["AuthSettings:JwtAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        services.AddAuthorization();
    }

    private void RegisterRavenDb(IServiceCollection services)
    {
        var ravenDbServer = Configuration["RavenDB_Server"];
        var ravenDbName = Configuration["RavenDB_DbName"];
        var ravenDbCertBase64 = Configuration["RavenDB_CertificateBase64"];
        var ravenDbCertPassword = Configuration["RavenDB_CertificatePassword"];
        if (ravenDbCertBase64 == null || ravenDbCertPassword == null || ravenDbServer == null || ravenDbName == null)
        {
            throw new InvalidOperationException("RavenDB configuration is missing");
        }
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
                if (type == typeof(ProfileDocument))
                    return "Profiles";
                if (type.ToString().EndsWith("Document"))
                {
                    return type.Name[..^"Document".Length] + "s";
                }
                return DocumentConventions.DefaultGetCollectionName(type);
            };

            var dateTimeService = sp.GetService<IDateTimeService>();
            store.OnBeforeStore += (sender, args) =>
            {
                if (args.Entity is IDateTracked dateTracked)
                {
                    // If DateCreated is the default value, it's a new document
                    if (dateTracked.DateCreated == DateTimeOffset.MinValue)
                    {
                        dateTracked.DateCreated = dateTimeService?.GetNow() ?? DateTimeOffset.UtcNow;
                    }
                    dateTracked.DateModified = dateTimeService?.GetNow() ?? DateTimeOffset.UtcNow;
                }
            };

            store.Initialize();

            // Déploiement automatique de l'index ProductTagsIndex au démarrage
            IndexCreation.CreateIndexes(typeof(ProductTagsIndex).Assembly, store);

            return store;
        });
        services.AddScoped(sp =>
        {
            var store = sp.GetRequiredService<IDocumentStore>();
            return store.OpenAsyncSession();
        });
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

        operation.Security ??= [];

        var scheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        };

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [scheme] = []
        });
    }
}

