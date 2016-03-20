using BuildBoard.Services;
using Microsoft.Practices.Unity;

namespace BuildBoard.Infrastructure
{
    public static class UnityDependencies
    {
        public static IUnityContainer Register()
        {
            UnityContainer container = new UnityContainer();

            container.RegisterType<IBoardService, BoardService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILocationProvider, LocationProvider>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}