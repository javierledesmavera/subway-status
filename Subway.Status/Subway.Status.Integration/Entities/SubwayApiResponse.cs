namespace Subway.Status.Integration.Entities
{
    public class SubwayApiResponse<THeader, TEntity>
        where THeader : SubwayResponseBaseHeader, new()
        where TEntity : SubwayBaseEntity, new()
    {
        public THeader Header { get; set; }
        public TEntity[] Entity { get; set; }
    }
}
