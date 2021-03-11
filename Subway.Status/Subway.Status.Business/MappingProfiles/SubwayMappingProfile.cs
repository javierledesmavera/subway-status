using AutoMapper;

namespace Subway.Status.Business.MappingProfiles
{
    public class SubwayMappingProfile : Profile
    {
        public SubwayMappingProfile()
        {
            CreateMap<Subway.Status.Integration.Entities.ServiceAlerts, Subway.Status.Domain.Dtos.ServiceAlerts>();
            CreateMap<Subway.Status.Integration.Entities.Header, Subway.Status.Domain.Dtos.Header>();
            CreateMap<Subway.Status.Integration.Entities.Entity, Subway.Status.Domain.Dtos.Entity>();
            CreateMap<Subway.Status.Integration.Entities.Alert, Subway.Status.Domain.Dtos.Alert>();
            CreateMap<Subway.Status.Integration.Entities.InformedEntity, Subway.Status.Domain.Dtos.InformedEntity>();
            CreateMap<Subway.Status.Integration.Entities.HeaderText, Subway.Status.Domain.Dtos.HeaderText>();
            CreateMap<Subway.Status.Integration.Entities.DescriptionText, Subway.Status.Domain.Dtos.DescriptionText>();
            CreateMap<Subway.Status.Integration.Entities.Translation, Subway.Status.Domain.Dtos.Translation>();
        }
    }
}
