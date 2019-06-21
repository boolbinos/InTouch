using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders
{
    public interface IExternalPostProviderFactory
    {
        IExternalPostProvider GetFacebookPostProvider();
        IExternalPostProvider GetSharepointPostProvider();
    }
}