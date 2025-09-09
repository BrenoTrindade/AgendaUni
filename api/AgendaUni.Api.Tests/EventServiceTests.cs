using Xunit;
using Moq;
using AgendaUni.Api.Services;
using AgendaUni.Api.Interfaces;
using AgendaUni.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgendaUni.Api.Tests
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _eventService = new EventService(_mockEventRepository.Object);
        }

        [Fact]
        public async Task GetAllEvents_ReturnsAllEvents()
        {
            // Arrange
            var events = new List<Event> { new Event { Id = 1 }, new Event { Id = 2 } };
            _mockEventRepository.Setup(repo => repo.GetAll()).ReturnsAsync(events);

            // Act
            var result = await _eventService.GetAllEvents();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<Event>)result).Count);
            _mockEventRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetEventById_ReturnsEvent_WhenEventExists()
        {
            // Arrange
            var @event = new Event { Id = 1 };
            _mockEventRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(@event);

            // Act
            var result = await _eventService.GetEventById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockEventRepository.Verify(repo => repo.GetById(1), Times.Once);
        }

        [Fact]
        public async Task AddEvent_AddsEventSuccessfully()
        {
            // Arrange
            var @event = new Event { Id = 1 };
            _mockEventRepository.Setup(repo => repo.Add(@event)).Returns(Task.CompletedTask);

            // Act
            await _eventService.AddEvent(@event);

            // Assert
            _mockEventRepository.Verify(repo => repo.Add(@event), Times.Once);
        }

        [Fact]
        public async Task UpdateEvent_UpdatesEventSuccessfully()
        {
            // Arrange
            var @event = new Event { Id = 1 };
            _mockEventRepository.Setup(repo => repo.Update(@event));

            // Act
            await _eventService.UpdateEvent(@event);

            // Assert
            _mockEventRepository.Verify(repo => repo.Update(@event), Times.Once);
        }

        [Fact]
        public async Task DeleteEvent_DeletesEventSuccessfully()
        {
            // Arrange
            var @event = new Event { Id = 1 };
            _mockEventRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(@event);
            _mockEventRepository.Setup(repo => repo.Delete(@event));

            // Act
            await _eventService.DeleteEvent(1);

            // Assert
            _mockEventRepository.Verify(repo => repo.GetById(1), Times.Once);
            _mockEventRepository.Verify(repo => repo.Delete(@event), Times.Once);
        }
    }
}