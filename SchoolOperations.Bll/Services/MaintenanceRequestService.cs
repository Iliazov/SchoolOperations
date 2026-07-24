using SchoolOperations.Bll.DTOs;
using SchoolOperations.Bll.Interfaces;
using SchoolOperations.Domain.Entities;
using SchoolOperations.Domain.Enums;

namespace SchoolOperations.Bll.Services
{
    public class MaintenanceRequestService : IMaintenanceRequestService
    {
        private readonly IMaintenanceRequestRepository _repository;
        private readonly IUserService _userService;
        public MaintenanceRequestService(
            IMaintenanceRequestRepository repository,
            IUserService userService) 
        {
            _repository = repository;
            _userService = userService;
        }
        public async Task<Guid> CreateAsync(CreateMaintenanceRequestDTO requestDTO)
        {
            ArgumentNullException.ThrowIfNull(requestDTO);
            if (string.IsNullOrWhiteSpace(requestDTO.Title))
                throw new ArgumentException("Title is required.");

            if (string.IsNullOrWhiteSpace(requestDTO.Location))
                throw new ArgumentException("Location is required.");

            if (string.IsNullOrWhiteSpace(requestDTO.Description))
                throw new ArgumentException("Description is required.");

            if (string.IsNullOrWhiteSpace(requestDTO.AssignedToUserId))
                throw new ArgumentException("Assignee is required.");

            if (!Enum.IsDefined(
                    typeof(RequestPriority),
                    requestDTO.Priority))
            {
                throw new ArgumentException("Invalid priority.");
            }
            var currentUser = await _userService.GetCurrentUserAsync();
            var assignableUsers = await _userService.GetAssignableUsersAsync();
            var assignedToUserId = requestDTO.AssignedToUserId.Trim();

            var assigneeExists = assignableUsers
                .Any(u => string.Equals(
                    u.Id, assignedToUserId, StringComparison.OrdinalIgnoreCase));
            if (!assigneeExists)
            {
                throw new ArgumentException("Selected assignee was not found");
            }
            var mainetanceRequest = new MaintenanceRequest
            {
                Id = Guid.NewGuid(),
                Title = requestDTO.Title.Trim(),
                Location = requestDTO.Location.Trim(),
                Description = requestDTO.Description.Trim(),
                Priority = requestDTO.Priority,
                Status = RequestStatus.New,
                CreatedByUserId = currentUser.Id,
                AssignedToUserId = assignedToUserId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            await _repository.AddAsync(mainetanceRequest);  
            await _repository.SaveChangesAsync();
            return mainetanceRequest.Id;
        }

        public async Task<List<MaintenanceRequestDTO>> GetMyTasksAsync()
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            var requests = await _repository.GetAssignedToAsync(currentUser.Id);
            return requests.Select(MapToDTO).ToList();
        }

        public async Task MarkAsReadAsync(Guid requestId)
        {
            if(requestId == Guid.Empty)
            {
                throw new ArgumentException("Request Id is required");
            }
            var currentUser = await _userService.GetCurrentUserAsync();
            var request = await _repository.GetByIdAsync(requestId);
            if (request == null)
            {
                throw new KeyNotFoundException("Request was not found");
            }
            if(!string.Equals(currentUser.Id, request.AssignedToUserId, StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException(
                    "You cannot access this request.");
            }
            if (request.IsRead)
                return;
            request.IsRead = true;
            await _repository.SaveChangesAsync();

        }

        private static MaintenanceRequestDTO MapToDTO(
            MaintenanceRequest request)
            {
                return new MaintenanceRequestDTO
                {
                    Id = request.Id,
                    Title = request.Title,
                    Location = request.Location,
                    Description = request.Description,
                    Priority = request.Priority,
                    Status = request.Status,
                    CreatedByUserId = request.CreatedByUserId,
                    AssignedToUserId = request.AssignedToUserId,
                    CreatedAt = request.CreatedAt,
                    IsRead = request.IsRead
                };
        }
    }
}
