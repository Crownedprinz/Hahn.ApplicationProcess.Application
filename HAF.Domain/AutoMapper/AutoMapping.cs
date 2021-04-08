using System;
namespace HAF.Domain.AutoMapper
{
    // AutoMapping.cs
    using AutoMapper;
    using global::AutoMapper;
    using HAF.Domain.Entities;
    using HAF.Domain.Resources;

    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<AddOrUpdateAssetResource, Asset>();
            CreateMap<Asset, AddOrUpdateAssetResource>();
        }
    }
}
