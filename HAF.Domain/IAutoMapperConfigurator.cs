using AutoMapper;

namespace  HAF.Domain
{
    public interface IAutoMapperConfigurator
    {
        void Configure(IMapperConfigurationExpression config);
    }
}