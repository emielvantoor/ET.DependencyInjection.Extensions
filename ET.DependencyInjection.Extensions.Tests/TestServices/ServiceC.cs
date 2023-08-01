namespace ET.DependencyInjection.Extensions.Tests.TestServices;

public class ServiceC : IOtherService
{
    public Task Do()
    {
        throw new NotImplementedException();
    }
}