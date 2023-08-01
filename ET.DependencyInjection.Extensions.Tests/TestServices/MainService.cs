namespace ET.DependencyInjection.Extensions.Tests.TestServices;

public class MainService: IService
{
    public IService Service1 { get; }
    public IService Service2 { get; }
    public IOtherService OtherService { get; }

    public MainService(IService service1, IService service2, IOtherService otherService)
    {
        Service1 = service1;
        Service2 = service2;
        OtherService = otherService;
    }

    public Task Do()
    {
        throw new NotImplementedException();
    }
}