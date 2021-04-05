using System;
using System.Collections.Generic;
using AutoMapper;
using HAF.Domain;

namespace HAF.CompositionRoot
{
    public class AutoMapperConfiguration : IAutoMapperConfigurator
    {
        private readonly IEnumerable<IAutoMapperConfigurator> _autoMapperConfigurators;

        public AutoMapperConfiguration(IEnumerable<IAutoMapperConfigurator> autoMapperConfigurators)
        {
            _autoMapperConfigurators = autoMapperConfigurators ?? throw new ArgumentNullException(nameof(autoMapperConfigurators));
        }

        public void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMissingTypeMaps = true;
            foreach (var autoMapperConfigurator in _autoMapperConfigurators)
                autoMapperConfigurator.Configure(config);
        }
    }
}