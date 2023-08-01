namespace ET.DependencyInjection.Extensions;

public class KeyedServiceDescriptor
{
    public object Key { get; }
    
    public Type ServiceType { get; }
    
    public Type ConcreteType { get; }
    
    public KeyedServiceDescriptor(object key, Type serviceType, Type concreteType)
    {
        Key = key;
        ServiceType = serviceType;
        ConcreteType = concreteType;
    }
}