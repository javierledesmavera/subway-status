using Subway.Status.Domain;

namespace Subway.Status.Integration.Entities
{
    public abstract class SubwayBaseEntity : IIdentificable
    {
        public string Id { get; set; }
    }
}
