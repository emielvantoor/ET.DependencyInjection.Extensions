namespace ET.DependencyInjection.Extensions;

public class NoConstructorsFoundException : Exception
{
    public NoConstructorsFoundException(Type concreteType)
        :base($"Could not find any usable constructor to instantiate {concreteType.FullName}")
    {
        
    }
}