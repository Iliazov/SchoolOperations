using SchoolOperations.Bll.DTOs;

namespace SchoolOperations.Bll.Interfaces
{
    public interface IUserService
    {
        public Task<AppUserDTO> GetCurrentUserAsync();
        public Task<List<AppUserDTO>> GetAssignableUsersAsync();
    }
}
