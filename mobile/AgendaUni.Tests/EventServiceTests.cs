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
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _eventService = new EventService(_mockEventRepository.Object);
        }

        [Fact]
        public async Task RegisterEventAsync_ShouldReturnSuccess_WhenEventIsValid()
        {
            // Arrange
            var newEvent = new Event
            {
                ClassId = 1,
                EventDate = DateTime.Now,
                Description = "Reunião"
            };

            _mockEventRepository
                .Setup(repo => repo.AddAsync(newEvent))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _eventService.RegisterEventAsync(newEvent);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Evento registrado com sucesso.", result.Message);
            _mockEventRepository.Verify(repo => repo.AddAsync(newEvent), Times.Once);
        }

        [Fact]
        public async Task RegisterEventAsync_ShouldReturnFailure_WhenClassIdIsZero()
        {
            // Arrange
            var newEvent = new Event
            {
                ClassId = 0,
                EventDate = DateTime.Now,
                Description = "Evento sem aula"
            };

            // Act
            var result = await _eventService.RegisterEventAsync(newEvent);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Selecione uma aula.", result.Message);
            _mockEventRepository.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never);
        }

        [Fact]
        public async Task RegisterEventAsync_ShouldReturnFailure_WhenDescriptionIsEmpty()
        {
            // Arrange
            var newEvent = new Event
            {
                ClassId = 1,
                EventDate = DateTime.Now,
                Description = ""
            };

            // Act
            var result = await _eventService.RegisterEventAsync(newEvent);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Informe a descrição do evento.", result.Message);
            _mockEventRepository.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never);
        }

        [Fact]
        public async Task RegisterEventAsync_ShouldReturnFailure_WhenDescriptionIsWhitespace()
        {
            // Arrange
            var newEvent = new Event
            {
                ClassId = 1,
                EventDate = DateTime.Now,
                Description = "   "
            };

            // Act
            var result = await _eventService.RegisterEventAsync(newEvent);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Informe a descrição do evento.", result.Message);
            _mockEventRepository.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never);
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

            _mockEventRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(eventList);

            // Act
            var result = await _eventService.GetAllEventsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockEventRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
    }
}
