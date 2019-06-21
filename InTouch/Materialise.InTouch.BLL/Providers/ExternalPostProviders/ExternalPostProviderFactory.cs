using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook;
using Microsoft.Extensions.Configuration;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint;

namespace Materialise.InTouch.BLL.Providers.ExternalPostProviders
{
    public class ExternalPostProviderFactory : IExternalPostProviderFactory
    {
        private readonly IExternalPostProvider[] _providers;

        public ExternalPostProviderFactory(IEnumerable<IExternalPostProvider> providers)
        {
            _providers = providers.ToArray();
        }

        public IExternalPostProvider GetFacebookPostProvider()
        {
            return _providers.FirstOrDefault(p => p is FacebookPostProvider) ?? throw new InvalidOperationException($"{nameof(FacebookPostProvider)} post provider not found.");
        }

        public IExternalPostProvider GetSharepointPostProvider()
        {
            return _providers.FirstOrDefault(p => p is SharepointPostProvider) ?? throw new InvalidOperationException($"{nameof(SharepointPostProvider)} post provider not found.");
        }
    }   
}