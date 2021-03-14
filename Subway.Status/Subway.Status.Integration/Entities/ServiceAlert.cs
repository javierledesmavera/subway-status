using Newtonsoft.Json;

namespace Subway.Status.Integration.Entities
{
    public class ServiceAlert : SubwayBaseEntity
    {
        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }
        [JsonProperty("Trip_update")]
        public object TripUpdate { get; set; }
        public object Vehicle { get; set; }
        public Alert Alert { get; set; }
    }

    public class Alert
    {
        [JsonProperty("active_period")]
        public object[] ActivePeriod { get; set; }
        [JsonProperty("informed_entity")]
        public InformedEntity[] InformedEntity { get; set; }
        public int Cause { get; set; }
        public int Effect { get; set; }
        public object Url { get; set; }
        [JsonProperty("header_text")]
        public TextInfo HeaderText { get; set; }
        [JsonProperty("description_text")]
        public TextInfo DescriptionText { get; set; }
    }

    public class TextInfo
    {
        public Translation[] Translation { get; set; }
    }

    public class Translation
    {
        public string Text { get; set; }
        public string Language { get; set; }
    }

    public class InformedEntity
    {
        [JsonProperty("agency_id")]
        public string AgencyId { get; set; }
        [JsonProperty("route_id")]
        public string RouteId { get; set; }
        [JsonProperty("route_type")]
        public int RouteType { get; set; }
        public object Trip { get; set; }
        [JsonProperty("stop_id")]
        public string StopId { get; set; }
    }
}
