namespace ET.DependencyInjection.Extensions;

public class ResolverConfiguration<TConcrete>
{
    internal Dictionary<string, object> KeyedParameters { get; } = new();
    
    public ResolverConfiguration<TConcrete> WithKeyedParameter(string name, object key)
    {
        KeyedParameters[name] = key;
        return this;
    }
}