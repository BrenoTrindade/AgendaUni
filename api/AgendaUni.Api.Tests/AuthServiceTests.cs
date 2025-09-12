using AgendaUni.Api.Interfaces;
using AgendaUni.Api.Models;
using AgendaUni.Api.Models.DTOs;
using AgendaUni.Api.Services;
using AgendaUni.Api.Services.Interfaces;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace AgendaUni.Api.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTokenService = new Mock<ITokenService>();
            _authService = new AuthService(_mockUserRepository.Object, _mockTokenService.Object);
        }

        [Fact]
        public async Task RegisterAsync_WithNewUser_ReturnsSuccess()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123"
            };

            _mockUserRepository.Setup(repo => repo.UsernameExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            _mockUserRepository.Setup(repo => repo.EmailExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            _mockUserRepository.Setup(repo => repo.Add(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _authService.RegisterAsync(registerDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuário registrado com sucesso", result.Message);
            Assert.NotNull(result.User);
            Assert.Equal(registerDto.Username, result.User.Username);
            Assert.Equal(registerDto.Email, result.User.Email);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingUsername_ReturnsFalse()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                Username = "existinguser",
                Email = "test@example.com",
                Password = "password123"
            };

            _mockUserRepository.Setup(repo => repo.UsernameExistsAsync("existinguser"))
                .ReturnsAsync(true);

            // Act
            var result = await _authService.RegisterAsync(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nome de usuário já está em uso", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ReturnsFalse()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                Username = "testuser",
                Email = "existing@example.com",
                Password = "password123"
            };

            _mockUserRepository.Setup(repo => repo.UsernameExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            _mockUserRepository.Setup(repo => repo.EmailExistsAsync("existing@example.com"))
                .ReturnsAsync(true);

            // Act
            var result = await _authService.RegisterAsync(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email já está em uso", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsSuccess()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Username = "testuser",
                Password = "password123"
            };

            var user = new User
            {
                Id = NUlid.Ulid.NewUlid().ToString(),
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow
            };

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync("testuser"))
                .ReturnsAsync(user);

            _mockUserRepository.Setup(repo => repo.Update(It.IsAny<User>()))
                .Verifiable();

            _mockTokenService.Setup(s => s.GenerateToken(It.IsAny<User>()))
                .Returns("dummy_token");

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Login realizado com sucesso", result.Message);
            Assert.NotNull(result.User);
            Assert.Equal(user.Id, result.User.Id);
            Assert.Equal(user.Username, result.User.Username);
            Assert.Equal(user.Email, result.User.Email);
            Assert.Equal("dummy_token", result.User.Token);
            _mockUserRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
            _mockTokenService.Verify(s => s.GenerateToken(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidUsername_ReturnsFalse()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Username = "nonexistentuser",
                Password = "password123"
            };

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync("nonexistentuser"))
                .ReturnsAsync((User)null);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nome de usuário ou senha incorretos", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ReturnsFalse()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Username = "testuser",
                Password = "wrongpassword"
            };

            var user = new User
            {
                Id = NUlid.Ulid.NewUlid().ToString(),
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow
            };

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync("testuser"))
                .ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nome de usuário ou senha incorretos", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithValidId_ReturnsUser()
        {
            // Arrange
            var userId = NUlid.Ulid.NewUlid().ToString();
            var user = new User
            {
                Id = userId,
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow
            };

            _mockUserRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            // Act
            var result = await _authService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("testuser", result.Username);
            Assert.Equal("test@example.com", result.Email);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var userId = NUlid.Ulid.NewUlid().ToString();

            _mockUserRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _authService.GetUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
        }
    }
}