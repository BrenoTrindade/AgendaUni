using Xunit;
using Moq;
using AgendaUni.Api.Services;
using AgendaUni.Api.Interfaces;
using AgendaUni.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgendaUni.Api.Tests
{
    public class ClassServiceTests
    {
        private readonly Mock<IClassRepository> _mockClassRepository;
        private readonly ClassService _classService;

        public ClassServiceTests()
        {
            _mockClassRepository = new Mock<IClassRepository>();
            _classService = new ClassService(_mockClassRepository.Object);
        }

        [Fact]
        public async Task GetAllClasses_ReturnsAllClasses()
        {
            // Arrange
            var classes = new List<Class> { new Class { Id = 1 }, new Class { Id = 2 } };
            _mockClassRepository.Setup(repo => repo.GetAll()).ReturnsAsync(classes);

            // Act
            var result = await _classService.GetAllClasses();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<Class>)result).Count);
            _mockClassRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetClassById_ReturnsClass_WhenClassExists()
        {
            // Arrange
            var @class = new Class { Id = 1 };
            _mockClassRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(@class);

            // Act
            var result = await _classService.GetClassById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockClassRepository.Verify(repo => repo.GetById(1), Times.Once);
        }

        [Fact]
        public async Task AddClass_AddsClassSuccessfully()
        {
            // Arrange
            var @class = new Class { Id = 1 };
            _mockClassRepository.Setup(repo => repo.Add(@class)).Returns(Task.CompletedTask);

            // Act
            await _classService.AddClass(@class);

            // Assert
            _mockClassRepository.Verify(repo => repo.Add(@class), Times.Once);
        }

        [Fact]
        public async Task UpdateClass_UpdatesClassSuccessfully()
        {
            // Arrange
            var @class = new Class { Id = 1 };
            _mockClassRepository.Setup(repo => repo.Update(@class));

            // Act
            await _classService.UpdateClass(@class);

            // Assert
            _mockClassRepository.Verify(repo => repo.Update(@class), Times.Once);
        }

        [Fact]
        public async Task DeleteClass_DeletesClassSuccessfully()
        {
            // Arrange
            var @class = new Class { Id = 1 };
            _mockClassRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(@class);
            _mockClassRepository.Setup(repo => repo.Delete(@class));

            // Act
            await _classService.DeleteClass(1);

            // Assert
            _mockClassRepository.Verify(repo => repo.GetById(1), Times.Once);
            _mockClassRepository.Verify(repo => repo.Delete(@class), Times.Once);
        }
    }
}