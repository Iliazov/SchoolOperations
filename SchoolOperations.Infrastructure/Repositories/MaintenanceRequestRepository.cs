using Microsoft.EntityFrameworkCore;
using SchoolOperations.Bll.Interfaces;
using SchoolOperations.DAL.Persistence;
using SchoolOperations.Domain.Entities;

namespace SchoolOperations.DAL.Repositories
{
    public class MaintenanceRequestRepository : IMaintenanceRequestRepository
    {
        private readonly AppDbContext _context;

        public MaintenanceRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MaintenanceRequest request)
        {
            await _context.MaintenanceRequests.AddAsync(request);
        }

        public async Task<List<MaintenanceRequest>> GetAssignedToAsync(string userId)
        {
            return await _context.MaintenanceRequests
                .AsNoTracking()
                .Where(x => x.AssignedToUserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<MaintenanceRequest?> GetByIdAsync(Guid id)
        {
            return await _context.MaintenanceRequests.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveChangesAsync()
        {

            await _context.SaveChangesAsync();
        }
    }
}
