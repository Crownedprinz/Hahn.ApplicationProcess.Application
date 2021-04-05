using AutoMapper;
//using HAF.Web.Resources;
using HAF.Domain;
using HAF.Domain.Entities;

namespace HAF.Web
{
    public class AutoMapperConfigurator : IAutoMapperConfigurator
    {
        public void Configure(IMapperConfigurationExpression config)
        {
        //    config.CreateMap<DebitorCreditor, DebitorCreditorResource>().PreserveReferences();
        //    config.CreateMap<Folder, FolderResource>().PreserveReferences();
        //    config.CreateMap<DocumentFlag, DocumentFlagResource>().PreserveReferences();
        //    config.CreateMap<DropscanRecipient, DropscanRecipientResource>().PreserveReferences();
        //    config.CreateMap<Company, CompanyResource>().PreserveReferences();
        //    config.CreateMap<DropscanMailing, DropscanMailingResource>().PreserveReferences();
        //    config.CreateMap<Document, DocumentResource>().PreserveReferences();
        //    config.CreateMap<JobStatus, string>().ConstructUsing(x => x.ToString());
        }
    }
}