using SchoolOperations.Bll.DTOs;

namespace SchoolOperations.Bll.Interfaces
{
    public interface IMaintenanceRequestService
    {
        public Task<Guid> CreateAsync(CreateMaintenanceRequestDTO requestDTO);
        public Task<List<MaintenanceRequestDTO>> GetMyTasksAsync();
        public Task MarkAsReadAsync(Guid requestId);

    }
}
