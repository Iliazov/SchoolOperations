using SchoolOperations.Domain.Entities;

namespace SchoolOperations.Bll.Interfaces
{
    public interface IMaintenanceRequestRepository
    {
        public Task AddAsync(MaintenanceRequest maintenanceRequestDTO);
        public Task<MaintenanceRequest?> GetByIdAsync(Guid id);
        public Task<List<MaintenanceRequest>> GetAssignedToAsync(string userId);
        Task SaveChangesAsync();
    }
}
