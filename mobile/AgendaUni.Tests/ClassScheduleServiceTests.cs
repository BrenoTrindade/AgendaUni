using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;
using AgendaUni.Services;
using Moq;
using Xunit;

namespace AgendaUni.Tests
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
        public async Task AddClassScheduleAsync_ShouldReturnSuccess_WhenScheduleIsValid()
        {
            // Arrange
            var newSchedule = new ClassSchedule { ClassId = 1, DayOfWeek = DayOfWeek.Monday, ClassTime = new TimeSpan(9, 0, 0) };

            // Act
            var result = await _classScheduleService.AddClassScheduleAsync(newSchedule);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Horário da aula registrado com sucesso.", result.Message);
            _mockClassScheduleRepository.Verify(repo => repo.AddAsync(newSchedule), Times.Once);
        }

        [Fact]
        public async Task AddClassScheduleAsync_ShouldReturnFailure_WhenClassIdIsZero()
        {
            // Arrange
            var newSchedule = new ClassSchedule { ClassId = 0, DayOfWeek = DayOfWeek.Monday, ClassTime = new TimeSpan(9, 0, 0) };

            // Act
            var result = await _classScheduleService.AddClassScheduleAsync(newSchedule);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Selecione uma aula.", result.Message);
            _mockClassScheduleRepository.Verify(repo => repo.AddAsync(It.IsAny<ClassSchedule>()), Times.Never);
        }

        [Fact]
        public async Task AddClassScheduleAsync_ShouldReturnFailure_WhenDayOfWeekIsInvalid()
        {
            // Arrange
            var invalidDayOfWeek = (DayOfWeek)999;
            var newSchedule = new ClassSchedule { ClassId = 1, DayOfWeek = invalidDayOfWeek, ClassTime = new TimeSpan(9, 0, 0) };

            // Act
            var result = await _classScheduleService.AddClassScheduleAsync(newSchedule);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Selecione um dia válido da semana.", result.Message);
            _mockClassScheduleRepository.Verify(repo => repo.AddAsync(It.IsAny<ClassSchedule>()), Times.Never);
        }

        [Fact]
        public async Task AddClassScheduleAsync_ShouldReturnFailure_WhenClassTimeIsDefault()
        {
            // Arrange
            var newSchedule = new ClassSchedule { ClassId = 1, DayOfWeek = DayOfWeek.Monday, ClassTime = default };

            // Act
            var result = await _classScheduleService.AddClassScheduleAsync(newSchedule);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Informe o horário da aula.", result.Message);
            _mockClassScheduleRepository.Verify(repo => repo.AddAsync(It.IsAny<ClassSchedule>()), Times.Never);
        }

        [Fact]
        public async Task UpdateClassScheduleAsync_ShouldReturnSuccess_WhenScheduleIsValid()
        {
            // Arrange
            var schedule = new ClassSchedule { Id = 1, ClassId = 1, DayOfWeek = DayOfWeek.Tuesday, ClassTime = new TimeSpan(10, 0, 0) };

            // Act
            var result = await _classScheduleService.UpdateClassScheduleAsync(schedule);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Horário da aula atualizado com sucesso.", result.Message);
            _mockClassScheduleRepository.Verify(repo => repo.UpdateAsync(schedule), Times.Once);
        }

        [Fact]
        public async Task DeleteClassScheduleAsync_ShouldReturnSuccess_WhenScheduleExists()
        {
            // Arrange
            var scheduleId = 1;
            var existingSchedule = new ClassSchedule { Id = scheduleId };
            _mockClassScheduleRepository.Setup(r => r.GetByIdAsync(scheduleId)).ReturnsAsync(existingSchedule);

            // Act
            var result = await _classScheduleService.DeleteClassScheduleAsync(scheduleId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Horário deletado com sucesso.", result.Message);
            _mockClassScheduleRepository.Verify(r => r.DeleteAsync(scheduleId), Times.Once);
        }

        [Fact]
        public async Task DeleteClassScheduleAsync_ShouldReturnFailure_WhenScheduleDoesNotExist()
        {
            // Arrange
            var scheduleId = 99;
            _mockClassScheduleRepository.Setup(r => r.GetByIdAsync(scheduleId)).ReturnsAsync((ClassSchedule)null);

            // Act
            var result = await _classScheduleService.DeleteClassScheduleAsync(scheduleId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Horário não encontrado.", result.Message);
            _mockClassScheduleRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetAllClassSchedulesAsync_ShouldReturnSchedules()
        {
            // Arrange
            var schedules = new List<ClassSchedule> { new ClassSchedule(), new ClassSchedule() };
            _mockClassScheduleRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(schedules);

            // Act
            var result = await _classScheduleService.GetAllClassSchedulesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockClassScheduleRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
    }
}
