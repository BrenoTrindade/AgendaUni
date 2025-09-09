using Xunit;
using Moq;
using AgendaUni.Api.Services;
using AgendaUni.Api.Interfaces;
using AgendaUni.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgendaUni.Api.Tests
{
    public class ClassScheduleServiceTests
    {
        private readonly Mock<IClassScheduleRepository> _mockClassScheduleRepository;
        private readonly ClassScheduleService _classScheduleService;

        public ClassScheduleServiceTests()
        {
            _mockClassScheduleRepository = new Mock<IClassScheduleRepository>();
            _classScheduleService = new ClassScheduleService(_mockClassScheduleRepository.Object);
        }

        [Fact]
        public async Task GetAllClassSchedules_ReturnsAllClassSchedules()
        {
            // Arrange
            var classSchedules = new List<ClassSchedule> { new ClassSchedule { Id = 1 }, new ClassSchedule { Id = 2 } };
            _mockClassScheduleRepository.Setup(repo => repo.GetAll()).ReturnsAsync(classSchedules);

            // Act
            var result = await _classScheduleService.GetAllClassSchedules();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<ClassSchedule>)result).Count);
            _mockClassScheduleRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetClassScheduleById_ReturnsClassSchedule_WhenClassScheduleExists()
        {
            // Arrange
            var classSchedule = new ClassSchedule { Id = 1 };
            _mockClassScheduleRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(classSchedule);

            // Act
            var result = await _classScheduleService.GetClassScheduleById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockClassScheduleRepository.Verify(repo => repo.GetById(1), Times.Once);
        }

        [Fact]
        public async Task AddClassSchedule_AddsClassScheduleSuccessfully()
        {
            // Arrange
            var classSchedule = new ClassSchedule { Id = 1 };
            _mockClassScheduleRepository.Setup(repo => repo.Add(classSchedule)).Returns(Task.CompletedTask);

            // Act
            await _classScheduleService.AddClassSchedule(classSchedule);

            // Assert
            _mockClassScheduleRepository.Verify(repo => repo.Add(classSchedule), Times.Once);
        }

        [Fact]
        public async Task UpdateClassSchedule_UpdatesClassScheduleSuccessfully()
        {
            // Arrange
            var classSchedule = new ClassSchedule { Id = 1 };
            _mockClassScheduleRepository.Setup(repo => repo.Update(classSchedule));

            // Act
            await _classScheduleService.UpdateClassSchedule(classSchedule);

            // Assert
            _mockClassScheduleRepository.Verify(repo => repo.Update(classSchedule), Times.Once);
        }

        [Fact]
        public async Task DeleteClassSchedule_DeletesClassScheduleSuccessfully()
        {
            // Arrange
            var classSchedule = new ClassSchedule { Id = 1 };
            _mockClassScheduleRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(classSchedule);
            _mockClassScheduleRepository.Setup(repo => repo.Delete(classSchedule));

            // Act
            await _classScheduleService.DeleteClassSchedule(1);

            // Assert
            _mockClassScheduleRepository.Verify(repo => repo.GetById(1), Times.Once);
            _mockClassScheduleRepository.Verify(repo => repo.Delete(classSchedule), Times.Once);
        }
    }
}