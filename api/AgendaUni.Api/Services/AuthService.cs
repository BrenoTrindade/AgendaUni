using AgendaUni.Api.Interfaces;
using AgendaUni.Api.Models;
using AgendaUni.Api.Models.DTOs;
using AgendaUni.Api.Services.Interfaces;
using BCrypt.Net;

namespace AgendaUni.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(bool Success, string Message, AuthResponseDTO? User)> RegisterAsync(RegisterDTO registerDto)
        {
            // Verificar se o nome de usuário já existe
            if (await _userRepository.UsernameExistsAsync(registerDto.Username))
            {
                return (false, "Nome de usuário já está em uso", null);
            }

            // Verificar se o email já existe
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
            {
                return (false, "Email já está em uso", null);
            }

            // Criar novo usuário
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = HashPassword(registerDto.Password),
                CreatedAt = DateTime.UtcNow
            };

            // Salvar usuário no banco de dados
            await _userRepository.Add(user);
            await _userRepository.Save();

            // Retornar resposta de sucesso
            return (true, "Usuário registrado com sucesso", MapToAuthResponse(user));
        }

        public async Task<(bool Success, string Message, AuthResponseDTO? User)> LoginAsync(LoginDTO loginDto)
        {
            // Buscar usuário pelo nome de usuário
            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);

            // Verificar se o usuário existe
            if (user == null)
            {
                return (false, "Nome de usuário ou senha incorretos", null);
            }

            // Verificar se a senha está correta
            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return (false, "Nome de usuário ou senha incorretos", null);
            }

            // Atualizar último login
            user.LastLogin = DateTime.UtcNow;
            _userRepository.Update(user);
            await _userRepository.Save();

            // Retornar resposta de sucesso
            return (true, "Login realizado com sucesso", MapToAuthResponse(user));
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            var users = await _userRepository.Find(u => u.Id == id);
            return users.FirstOrDefault();
        }

        // Método para mapear User para AuthResponseDTO
        private static AuthResponseDTO MapToAuthResponse(User user)
        {
            return new AuthResponseDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin
            };
        }

        // Método para criar hash da senha
        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        // Método para verificar senha
        private static bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}