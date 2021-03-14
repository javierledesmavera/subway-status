namespace Subway.Status.Domain.Dtos
{
    public class Stop : IIdentificable
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
