using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ET.DependencyInjection.Extensions;

public static class ServiceProviderExtensions
{
    public static TService GetKeyedService<TService>(this IServiceProvider serviceProvider, object key)
    {
        return (TService) GetKeyedService(serviceProvider, typeof(TService), key);
    }

    public static object GetKeyedService(this IServiceProvider serviceProvider, Type type, object key)
    {
        var services = (IEnumerable<KeyedServiceDescriptor>) serviceProvider.GetServices(typeof(KeyedServiceDescriptor));
        var typedServiceDescriptor = services.First(service => service?.ServiceType == type && key.Equals(service?.Key));
        return serviceProvider.GetRequiredService(typedServiceDescriptor.ConcreteType);
    }
    
    public static TConcrete Construct<TConcrete>(this IServiceProvider serviceProvider, Action<ResolverConfiguration<TConcrete>> configure)
    {
        var resolverConfiguration = new ResolverConfiguration<TConcrete>();
        configure(resolverConfiguration);

        var keyedConstructorParameters = resolverConfiguration.KeyedParameters;
        var serviceType = typeof(TConcrete);
        var implementingTypes = serviceType.GetInterfaces();
        var constructors = serviceType.GetConstructors();
        
        foreach (var constructorInfo in constructors)
        {
            if (!TryResolveConstructorParameters(implementingTypes, constructorInfo, serviceProvider, keyedConstructorParameters, out var dependencies))
            {
                continue;
            }
            
            var instance = Activator.CreateInstance(typeof(TConcrete), dependencies.ToArray());
            if (instance == null)
            {
                continue;
            }
            
            return (TConcrete) instance;
        }

        throw new NoConstructorsFoundException(serviceType);
    }

    private static bool TryResolveConstructorParameters(ICollection<Type> implementingServiceTypes, ConstructorInfo constructorInfo, IServiceProvider serviceProvider, IReadOnlyDictionary<string, object> keyedParameters, out IEnumerable<object> dependencies)
    {
        var foundDependencies = new List<object>();
        var requiredParameters = constructorInfo.GetParameters();
        foreach (var requiredParameter in requiredParameters)
        {
            var service = GetService(implementingServiceTypes, serviceProvider, keyedParameters, requiredParameter);
            if (service == null)
            {
                break;
            }

            foundDependencies.Add(service);
        }

        dependencies = foundDependencies;
        return requiredParameters.Length == foundDependencies.Count;
    }

    private static object GetService(ICollection<Type> implementingServiceTypes, IServiceProvider serviceProvider, IReadOnlyDictionary<string, object> keyedParameters, ParameterInfo requiredParameter)
    {
        object service;
        var keyedParameterValue = keyedParameters.ContainsKey(requiredParameter.Name) ? keyedParameters[requiredParameter.Name] : null;
        if (keyedParameterValue != null)
        {
            service = serviceProvider.GetKeyedService(requiredParameter.ParameterType, keyedParameterValue);
        }
        else
        {
            if (implementingServiceTypes.Contains(requiredParameter.ParameterType))
            {
                throw new InvalidOperationException($"Parameter {requiredParameter.Name} is of the same type {requiredParameter.ParameterType} as the requested service. Use a keyed parameter to solve.");
            }

            service = serviceProvider.GetService(requiredParameter.ParameterType);
        }

        return service;
    }
}