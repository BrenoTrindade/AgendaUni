using AgendaUni.Api.Models;
using AgendaUni.Api.Models.DTOs;

namespace AgendaUni.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, AuthResponseDTO? User)> RegisterAsync(RegisterDTO registerDto);
        Task<(bool Success, string Message, AuthResponseDTO? User)> LoginAsync(LoginDTO loginDto);
        Task<User?> GetUserByIdAsync(string id);
    }
}