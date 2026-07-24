using SchoolOperations.Domain.Enums;
namespace SchoolOperations.Domain.Entities
{
    public class MaintenanceRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RequestPriority Priority { get; set; }
        public RequestStatus Status { get; set; }
        public string CreatedByUserId { get; set; } = string.Empty;
        public string AssignedToUserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
