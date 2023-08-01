using ET.DependencyInjection.Extensions.Tests.TestServices;
using Microsoft.Extensions.DependencyInjection;

namespace ET.DependencyInjection.Extensions.Tests;

public class ServiceCollectionExtensionsTests
{
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();
    
    [Fact]
    public void GivenKeyedServiceAAndB_WhenGetRequiredServiceKeyedServiceA_ThenReturnServiceA()
    {
        // Arrange
        _serviceCollection.AddKeyedTransient<IService, ServiceA>(ServiceKeys.ServiceA);
        _serviceCollection.AddKeyedTransient<IService, ServiceB>(ServiceKeys.ServiceB);
        
        // Act
        var result = _serviceCollection.BuildServiceProvider()
            .GetKeyedService<IService>(ServiceKeys.ServiceA);
        
        // Assert
        Assert.IsType<ServiceA>(result);
    }
    
    [Fact]
    public void GivenServiceRequiresKeyedParameters_WhenGetRequiredService_ThenKeyedServicesGetsResolved()
    {
        // Arrange
        _serviceCollection.AddKeyedTransient<IService, ServiceA>(ServiceKeys.ServiceA);
        _serviceCollection.AddKeyedTransient<IService, ServiceB>(ServiceKeys.ServiceB);
        _serviceCollection.AddTransient<IOtherService, ServiceC>();
        _serviceCollection.AddKeyedTransientWithKeyedParameters<IService, MainService>(resolver => resolver
                .WithKeyedParameter("service1", ServiceKeys.ServiceA)
                .WithKeyedParameter("service2", ServiceKeys.ServiceB));
        
        // Act
        var service = _serviceCollection.BuildServiceProvider().GetRequiredService<IService>();
        
        // Assert
        Assert.IsType<MainService>(service);
        
        var mainService = (MainService) service;
        Assert.IsType<ServiceA>(mainService.Service1);
        Assert.IsType<ServiceB>(mainService.Service2);
        Assert.IsType<ServiceC>(mainService.OtherService);
    }

    [Fact]
    public void GivenAAlreadyAddedService_WhenAddKeyedServiceDescriptor_ThenServiceIsAlsoKeyed()
    {
        // Arrange
        const string theServiceKey = "";
        _serviceCollection.AddTransient<ServiceA>();
        _serviceCollection.AddKeyedServiceDescriptor<IService, ServiceA>(theServiceKey);
        
        // Act
        var service = _serviceCollection.BuildServiceProvider().GetKeyedService<IService>(theServiceKey);
        
        // Assert
        Assert.IsType<ServiceA>(service);
    }
}