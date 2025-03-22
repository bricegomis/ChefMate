using ChefMate.API.Manager;
using ChefMate.API.Services;
using Microsoft.OpenApi.Models;
using Serilog;

namespace ChefMate.API;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        // Init configuration
        var mongoDbConnectionString = Configuration["MongoDB_ConnectionString"];
        var dbName = Configuration["MongoDB_DbName"];

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChefMate.API", Version = "v1" });
        });

        if (string.IsNullOrEmpty(mongoDbConnectionString)
            || string.IsNullOrEmpty(dbName))
        {
            throw new Exception("MongoDB Connection string & DbName not found");
        }

        var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(Log.Logger));
        var datetimeProvider = new DateTimeProvider();

        var mongoDBService = new MongoDBService(loggerFactory.CreateLogger<MongoDBService>(),
                                                datetimeProvider,
                                                mongoDbConnectionString,
                                                dbName);
        services.AddSingleton<IMongoDBService>(mongoDBService);

        var profilManager = new ProfileManager(loggerFactory.CreateLogger<IProfileManager>(),
                                               mongoDBService,
                                               datetimeProvider);
        services.AddSingleton<IProfileManager>(profilManager);
        profilManager.InitDefaultProfile().GetAwaiter().GetResult();
        var productManager = new ProductManager(loggerFactory.CreateLogger<IProductManager>(),
                                            mongoDBService,
                                            profilManager,
                                            datetimeProvider);
        
        services.AddSingleton<IProductManager>(productManager);
        //services.AddSingleton<IHostedService>(provider => manager);

        // Configure OAuth authentication
        //services.AddAuthentication(options =>
        //{
        //    options.DefaultScheme = "Cookies";
        //    options.DefaultChallengeScheme = "oidc";
        //})
        //.AddCookie("Cookies")
        //.AddOpenIdConnect("oidc", options =>
        //{
        //    options.Authority = "https://your-oauth-provider.com";
        //    options.ClientId = "your-client-id";
        //    options.ClientSecret = "your-client-secret";
        //    options.ResponseType = "code";
        //});
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            //app.UseSwaggerUI();
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
