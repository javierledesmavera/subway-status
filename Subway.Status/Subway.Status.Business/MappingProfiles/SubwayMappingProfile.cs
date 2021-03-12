using AutoMapper;
using Subway.Status.Integration.Entities;
using System.Linq;

namespace Subway.Status.Business.MappingProfiles
{
    public class SubwayMappingProfile : Profile
    {
        public SubwayMappingProfile()
        {
            CreateMap<Subway.Status.Integration.Entities.SubwayApiResponse<ServiceAlertsHeader, Subway.Status.Integration.Entities.ServiceAlerts>, Subway.Status.Domain.Dtos.ServiceAlert>()
                .ForMember(dto => dto.Alerts, mapper => mapper.MapFrom(entity => entity.Entity));

            CreateMap<Subway.Status.Integration.Entities.ServiceAlerts, Domain.Dtos.Alert>()
                .ForMember(dto => dto.Id, cfg => cfg.MapFrom(entity => entity.Id))
                .ForMember(dto => dto.RouteId, cfg => cfg.MapFrom(entity => entity.Alert.InformedEntity.FirstOrDefault().RouteId))
                .ForMember(dto => dto.StopId, cfg => cfg.MapFrom(entity => entity.Alert.InformedEntity.FirstOrDefault().StopId))
                .ForMember(dto => dto.HeaderText, cfg => cfg.MapFrom(entity => entity.Alert.HeaderText.Translation.FirstOrDefault(t => t.Language == "es").Text))
                .ForMember(dto => dto.DescriptionText, cfg => cfg.MapFrom(entity => entity.Alert.DescriptionText.Translation.FirstOrDefault(t => t.Language == "es").Text))
                .ForMember(dto => dto.Cause, cfg => cfg.MapFrom(entity => entity.Alert.Cause))
                .ForMember(dto => dto.Effect, cfg => cfg.MapFrom(entity => entity.Alert.Effect));
        }
    }
}
