using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using FluentValidation;
using HAF.DAL;
using HAF.Domain;
using HAF.Domain.Entities;
using HAF.Domain.Services;
using HAF.Domain.Validators;
using SimpleInjector;

namespace HAF.CompositionRoot
{
    internal static class CompositionRoot
    {
        public static void Configure(Container container)
        {
            var relevantAssemblies = RegistrationExtensions.GetMatchingAssemblies("HAF.*.dll", "HAF.*.exe").ToArray();
            container.RegisterQueries(relevantAssemblies);
            container.RegisterCommands(relevantAssemblies);
            container.RegisterStartUpAndShutDownHandler(relevantAssemblies);
            container.RegisterAutoMapper(relevantAssemblies);
            container.Register<IAssetService, AssetService>();
            container.RegisterSingleton<IValidator<Asset>, AssetValidator>();
            container.RegisterSingleton<IRecurringJobManager, NullRecurringJobManager>();

            Database.SetInitializer(new DatabaseInitializer());
        }

        private static void RegisterAutoMapper(this Container container, IEnumerable<Assembly> relevantAssemblies)
        {
            container.RegisterCollection<IAutoMapperConfigurator>(relevantAssemblies);
            container.RegisterSingleton<IAutoMapperConfigurator, AutoMapperConfiguration>();
        }

        private static void RegisterCommands(this Container container, IEnumerable<Assembly> relevantAssemblies)
        {
            container.Register(typeof(ICommand<>), relevantAssemblies);
        }

        private static void RegisterQueries(this Container container, ICollection<Assembly> relevantAssemblies)
        {
            container.Register(typeof(IQuery<,>), relevantAssemblies);
            container.Register(typeof(IQueryAll<>), relevantAssemblies);
            container.Register(typeof(IQuerySingle<>), relevantAssemblies);
        }

        private static void RegisterStartUpAndShutDownHandler(
            this Container container,
            ICollection<Assembly> relevantAssemblies)
        {
            container.RegisterCollection<IApplicationStartUpHandler>(relevantAssemblies);
            container.RegisterSingleton<IApplicationStartUpHandler, CompositeStartUpHandler>();
            container.RegisterCollection<IApplicationShutDownHandler>(relevantAssemblies);
            container.Register<IApplicationShutDownHandler, CompositeShutDownHandler>();
        }
    }
}