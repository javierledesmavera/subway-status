using Microsoft.EntityFrameworkCore;
using Subway.Status.Repository.Entities;

namespace Subway.Status.Repository
{
    public class SubwayContext : DbContext
    {
        public SubwayContext(DbContextOptions<SubwayContext> options) : base(options)
        {
        }

        public DbSet<Alert> Alerts { get; set; }
    }
}
