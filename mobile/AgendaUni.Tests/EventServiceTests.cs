using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;
using AgendaUni.Services;
using AgendaUni.Common;
using Moq;
using Xunit;

namespace AgendaUni.Tests
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly Mock<IClassRepository> _mockClassRepository;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _mockClassRepository = new Mock<IClassRepository>();
            _mockNotificationService = new Mock<INotificationService>();
            _eventService = new EventService(_mockEventRepository.Object, _mockClassRepository.Object, _mockNotificationService.Object);
        }

        [Fact]
        public async Task AddEventAsync_ShouldReturnSuccess_WhenEventIsValid()
        {
            // Arrange
            var newEvent = new Event { ClassId = 1, EventDate = DateTime.Now, Description = "Reunião" };
            var classToReturn = new Class { Id = 1, ClassName = "Test Class" };
            var notificationIds = new List<int> { 1, 2, 3 };

            _mockClassRepository.Setup(repo => repo.GetByIdAsync(newEvent.ClassId)).ReturnsAsync(classToReturn);
            _mockNotificationService.Setup(service => service.ScheduleNotificationForEvent(newEvent, classToReturn)).ReturnsAsync(notificationIds);
            _mockEventRepository.Setup(repo => repo.AddAsync(newEvent, notificationIds)).Returns(Task.CompletedTask);

            // Act
            var result = await _eventService.AddEventAsync(newEvent);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Evento registrado com sucesso.", result.Message);
            _mockEventRepository.Verify(repo => repo.AddAsync(newEvent, notificationIds), Times.Once);
        }

        [Fact]
        public async Task AddEventAsync_ShouldReturnFailure_WhenClassIdIsZero()
        {
            // Arrange
            var newEvent = new Event { ClassId = 0, EventDate = DateTime.Now, Description = "Evento sem aula" };

            // Act
            var result = await _eventService.AddEventAsync(newEvent);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Selecione uma aula.", result.Message);
            _mockEventRepository.Verify(repo => repo.AddAsync(It.IsAny<Event>(), It.IsAny<IEnumerable<int>>()), Times.Never);
        }

        [Fact]
        public async Task AddEventAsync_ShouldReturnFailure_WhenDescriptionIsEmpty()
        {
            // Arrange
            var newEvent = new Event { ClassId = 1, EventDate = DateTime.Now, Description = "" };

            // Act
            var result = await _eventService.AddEventAsync(newEvent);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Informe a descrição do evento.", result.Message);
            _mockEventRepository.Verify(repo => repo.AddAsync(It.IsAny<Event>(), It.IsAny<IEnumerable<int>>()), Times.Never);
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldReturnSuccess_WhenEventIsValid()
        {
            // Arrange
            var eventToUpdate = new Event { Id = 1, ClassId = 1, Description = "Updated Description" };
            var existingEvent = new Event { Id = 1, ClassId = 1, Description = "Old Description", NotificationIds = new List<EventNotification> { new EventNotification { NotificationId = 1 } } };
            var classToReturn = new Class { Id = 1, ClassName = "Test Class" };
            var notificationIds = new List<int> { 2, 3, 4 };

            _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventToUpdate.Id)).ReturnsAsync(existingEvent);
            _mockClassRepository.Setup(repo => repo.GetByIdAsync(eventToUpdate.ClassId)).ReturnsAsync(classToReturn);
            _mockNotificationService.Setup(service => service.ScheduleNotificationForEvent(eventToUpdate, classToReturn)).ReturnsAsync(notificationIds);
            _mockEventRepository.Setup(repo => repo.UpdateAsync(eventToUpdate, notificationIds)).Returns(Task.CompletedTask);

            // Act
            var result = await _eventService.UpdateEventAsync(eventToUpdate);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Evento atualizado com sucesso.", result.Message);
            _mockNotificationService.Verify(service => service.CancelNotification(1), Times.Once);
            _mockEventRepository.Verify(r => r.UpdateAsync(eventToUpdate, notificationIds), Times.Once);
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldReturnFailure_WhenDescriptionIsEmpty()
        {
            // Arrange
            var eventToUpdate = new Event { Id = 1, ClassId = 1, Description = "" };

            // Act
            var result = await _eventService.UpdateEventAsync(eventToUpdate);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Informe a descrição do evento.", result.Message);
            _mockEventRepository.Verify(r => r.UpdateAsync(It.IsAny<Event>(), It.IsAny<IEnumerable<int>>()), Times.Never);
        }

        [Fact]
        public async Task DeleteEventAsync_ShouldReturnSuccess_WhenEventExists()
        {
            // Arrange
            var eventId = 1;
            var existingEvent = new Event { Id = eventId, NotificationIds = new List<EventNotification> { new EventNotification { NotificationId = 1 } } };
            _mockEventRepository.Setup(r => r.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);

            // Act
            var result = await _eventService.DeleteEventAsync(eventId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Evento deletado com sucesso.", result.Message);
            _mockNotificationService.Verify(service => service.CancelNotification(1), Times.Once);
            _mockEventRepository.Verify(r => r.DeleteAsync(eventId), Times.Once);
        }

        [Fact]
        public async Task DeleteEventAsync_ShouldReturnFailure_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 99;
            _mockEventRepository.Setup(r => r.GetByIdAsync(eventId)).ReturnsAsync((Event)null);

            // Act
            var result = await _eventService.DeleteEventAsync(eventId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Evento não encontrado.", result.Message);
            _mockEventRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetAllEventsAsync_ShouldReturnListOfEvents()
        {
            // Arrange
            var eventList = new List<Event>
            {
                new Event { Id = 1, ClassId = 1, EventDate = DateTime.Now, Description = "Evento 1" },
                new Event { Id = 2, ClassId = 2, EventDate = DateTime.Now, Description = "Evento 2" }
            };

            _mockEventRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(eventList);

            // Act
            var result = await _eventService.GetAllEventsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockEventRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
    }
}
