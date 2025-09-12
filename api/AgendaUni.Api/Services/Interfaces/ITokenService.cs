using AgendaUni.Api.Models;
using System.Security.Claims;

namespace AgendaUni.Api.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}