using ChefMate.API.Attributes;
using System.Reflection;

namespace ChefMate.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAttributedServices(this IServiceCollection services, params Assembly[]? assemblies)
    {
        // Vérifie si assemblies est null ou vide, et utilise l'assembly courant si besoin
        if (assemblies == null || assemblies.Length == 0)
            assemblies = [Assembly.GetExecutingAssembly()];

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttributes<InjectableAttribute>().Any());

            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<InjectableAttribute>();
                var interfaces = type.GetInterfaces();
                var serviceType = interfaces.FirstOrDefault() ?? type;

                var lifetime = attr?.Lifetime ?? ServiceLifetime.Scoped;
                switch (lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(serviceType, type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(serviceType, type);
                        break;
                    default:
                        services.AddScoped(serviceType, type);
                        break;
                }
            }
        }
    }
}