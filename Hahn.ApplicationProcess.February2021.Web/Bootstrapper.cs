using System.Web.Mvc;
using Hahn.ApplicationProcess.February2021.Web.Controllers;
using Hahn.ApplicationProcess.February2021.Web.Models;
using Microsoft.Practices.Unity;
using Unity.Mvc3;

namespace Hahn.ApplicationProcess.February2021.Web
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();            
            container.RegisterType<ResourceApiController<Asset, AssetResource>, AssetController>();

            return container;
        }
    }
}