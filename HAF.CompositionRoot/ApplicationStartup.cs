using System;
using AutoMapper;
using HAF.Domain;
using SimpleInjector;

namespace HAF.CompositionRoot
{
    public static class ApplicationStartup
    {
        public static Action<Container> AfterInternalConfiguration { get; set; }
        public static Action<Container> AfterVerification { get; set; }
        public static Action<Container> BeforeInternalConfiguration { get; set; }

        public static void Execute()
        {
            var container = new Container();
            BeforeInternalConfiguration?.Invoke(container);
            CompositionRoot.Configure(container);
            AfterInternalConfiguration?.Invoke(container);
            container.Verify();
            AfterVerification?.Invoke(container);
            Mapper.Initialize(container.GetInstance<IAutoMapperConfigurator>().Configure);
            container.GetInstance<IApplicationStartUpHandler>().Handle();
        }
    }
}