
namespace Subway.Status.Domain.Dtos
{
    public class ServiceAlerts
    {
        public Header Header { get; set; }
        public Entity[] Entity { get; set; }
    }

    public class Header
    {
        public string GtfsRealtimeVersion { get; set; }
        public int Incrementality { get; set; }
        public int Timestamp { get; set; }
    }

    public class Entity
    {
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public object TripUpdate { get; set; }
        public object Vehicle { get; set; }
        public Alert Alert { get; set; }
    }

    public class Alert
    {
        public object[] ActivePeriod { get; set; }
        public InformedEntity[] InformedEntity { get; set; }
        public int Cause { get; set; }
        public int Effect { get; set; }
        public object Url { get; set; }
        public HeaderText HeaderText { get; set; }
        public DescriptionText DescriptionText { get; set; }
    }

    public class HeaderText
    {
        public Translation[] Translation { get; set; }
    }

    public class Translation
    {
        public string Text { get; set; }
        public string Language { get; set; }
    }

    public class DescriptionText
    {
        public Translation[] Translation { get; set; }
    }

    public class InformedEntity
    {
        public string AgencyId { get; set; }
        public string RouteId { get; set; }
        public int RouteType { get; set; }
        public object trip { get; set; }
        public string StopId { get; set; }
    }
}
