using Microsoft.EntityFrameworkCore;
using SchoolOperations.Domain.Entities;

namespace SchoolOperations.DAL.Persistence
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
        public DbSet<MaintenanceRequest> MaintenanceRequests => Set<MaintenanceRequest>();
    }
}
