using SchoolOperations.Domain.Enums;

namespace SchoolOperations.Bll.DTOs
{
    public class CreateMaintenanceRequestDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RequestPriority Priority { get; set; } = RequestPriority.Medium;
        public string AssignedToUserId { get; set; } = string.Empty;
    }
}
