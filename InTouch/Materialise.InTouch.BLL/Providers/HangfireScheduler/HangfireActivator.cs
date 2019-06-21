using System;
using System.Collections.Generic;
using System.Text;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Materialise.InTouch.BLL.Providers
{
    public class HangfireActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public HangfireActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type type)
        {
            var scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            var job = scope.ServiceProvider.GetService(type);
            if (job == null)
                throw new Exception($"Could not activate a job of type '{type.FullName}'.");

            return job;
        }
    }
}
