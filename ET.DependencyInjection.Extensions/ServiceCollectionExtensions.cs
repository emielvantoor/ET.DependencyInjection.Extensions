using Microsoft.Extensions.DependencyInjection;

namespace ET.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddKeyedTransient<TService, TConcrete>(this IServiceCollection serviceCollection, object key) 
        where TService : class where TConcrete : class, TService
    {
        serviceCollection.AddTransient<TConcrete>();
        serviceCollection.AddKeyedServiceDescriptor<TService, TConcrete>(key);
    }
    
    public static void AddKeyedScoped<TService, TConcrete>(this IServiceCollection serviceCollection, object key) 
        where TService : class where TConcrete : class, TService
    {
        serviceCollection.AddScoped<TConcrete>();
        serviceCollection.AddKeyedServiceDescriptor<TService, TConcrete>(key);
    }
    
    public static void AddKeyedSingleton<TService, TConcrete>(this IServiceCollection serviceCollection, object key) 
        where TService : class where TConcrete : class, TService
    {
        serviceCollection.AddSingleton<TConcrete>();
        serviceCollection.AddKeyedServiceDescriptor<TService, TConcrete>(key);
    }

    public static void AddKeyedServiceDescriptor<TService, TConcrete>(this IServiceCollection serviceCollection, object key)
    {
        serviceCollection.AddTransient<KeyedServiceDescriptor, KeyedServiceDescriptor>(_ => new KeyedServiceDescriptor(key, typeof(TService), typeof(TConcrete)));
    }
    
    public static void AddKeyedTransientWithKeyedParameters<TService, TConcrete>(this IServiceCollection serviceCollection,  Action<ResolverConfiguration<TConcrete>> configure) 
        where TService : class where TConcrete : class, TService
    {
        serviceCollection.AddTransient<TService, TConcrete>(provider => provider.Construct(configure));
    }
    
    public static void AddKeyedScopedWithKeyedParameters<TService, TConcrete>(this IServiceCollection serviceCollection,  Action<ResolverConfiguration<TConcrete>> configure) 
        where TService : class where TConcrete : class, TService
    {
        serviceCollection.AddScoped<TService, TConcrete>(provider => provider.Construct(configure));
    }
    
    public static void AddKeyedSingletonWithKeyedParameters<TService, TConcrete>(this IServiceCollection serviceCollection,  Action<ResolverConfiguration<TConcrete>> configure) 
        where TService : class where TConcrete : class, TService
    {
        serviceCollection.AddSingleton<TService, TConcrete>(provider => provider.Construct(configure));
    }
}