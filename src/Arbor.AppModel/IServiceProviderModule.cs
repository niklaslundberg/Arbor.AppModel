using Arbor.AppModel.Hosting;

namespace Arbor.AppModel
{
    public interface IServiceProviderModule
    {
        void Register(ServiceProviderHolder serviceProviderHolder);
    }
}