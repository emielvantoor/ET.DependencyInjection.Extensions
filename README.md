# ET.DependencyInjection.Extensions

An example implementation of keyed services into the AspNetCore ServiceCollection.

### How to use

Register your keyed service using

    _serviceCollection.AddKeyedTransient<IService, ServiceA>(ServiceKeys.ServiceA);
    _serviceCollection.AddKeyedTransient<IService, ServiceB>(ServiceKeys.ServiceB);

Now register the service that will use the keyed services.

    _serviceCollection.AddKeyedTransientWithKeyedParameters<IService, MainService>(resolver => resolver
            .WithKeyedParameter("service1", ServiceKeys.ServiceA)
            .WithKeyedParameter("service2", ServiceKeys.ServiceB));

Rename `service1` and `service2` to valid constructor parameter names in your service