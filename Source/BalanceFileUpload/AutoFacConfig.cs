
using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;

namespace BalanceFileUpload
{
    public static class AutoFacConfig
    {
        private static IContainer _container;

        public static void Initialize(IConfiguration configuration)
        {
            var iocContainerBuilder = new ContainerBuilder();
            iocContainerBuilder.RegisterModule(new ConfigurationModule(configuration));
            iocContainerBuilder.RegisterInstance(configuration);
            _container = iocContainerBuilder.Build();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
