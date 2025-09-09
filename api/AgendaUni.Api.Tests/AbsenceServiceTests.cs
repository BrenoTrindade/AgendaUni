using Xunit;
using Moq;
using AgendaUni.Api.Services;
using AgendaUni.Api.Interfaces;
using AgendaUni.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgendaUni.Api.Tests
{
    public class AbsenceServiceTests
    {
        private readonly Mock<IAbsenceRepository> _mockAbsenceRepository;
        private readonly AbsenceService _absenceService;

        public AbsenceServiceTests()
        {
            _mockAbsenceRepository = new Mock<IAbsenceRepository>();
            _absenceService = new AbsenceService(_mockAbsenceRepository.Object);
        }

        [Fact]
        public async Task GetAllAbsences_ReturnsAllAbsences()
        {
            // Arrange
            var absences = new List<Absence> { new Absence { Id = 1 }, new Absence { Id = 2 } };
            _mockAbsenceRepository.Setup(repo => repo.GetAll()).ReturnsAsync(absences);

            // Act
            var result = await _absenceService.GetAllAbsences();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<Absence>)result).Count);
            _mockAbsenceRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetAbsenceById_ReturnsAbsence_WhenAbsenceExists()
        {
            // Arrange
            var absence = new Absence { Id = 1 };
            _mockAbsenceRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(absence);

            // Act
            var result = await _absenceService.GetAbsenceById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockAbsenceRepository.Verify(repo => repo.GetById(1), Times.Once);
        }

        [Fact]
        public async Task AddAbsence_AddsAbsenceSuccessfully()
        {
            // Arrange
            var absence = new Absence { Id = 1 };
            _mockAbsenceRepository.Setup(repo => repo.Add(absence)).Returns(Task.CompletedTask);

            // Act
            await _absenceService.AddAbsence(absence);

            // Assert
            _mockAbsenceRepository.Verify(repo => repo.Add(absence), Times.Once);
        }

        [Fact]
        public async Task UpdateAbsence_UpdatesAbsenceSuccessfully()
        {
            // Arrange
            var absence = new Absence { Id = 1 };
            _mockAbsenceRepository.Setup(repo => repo.Update(absence));

            // Act
            await _absenceService.UpdateAbsence(absence);

            // Assert
            _mockAbsenceRepository.Verify(repo => repo.Update(absence), Times.Once);
        }

        [Fact]
        public async Task DeleteAbsence_DeletesAbsenceSuccessfully()
        {
            // Arrange
            var absence = new Absence { Id = 1 };
            _mockAbsenceRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(absence);
            _mockAbsenceRepository.Setup(repo => repo.Delete(absence));

            // Act
            await _absenceService.DeleteAbsence(1);

            // Assert
            _mockAbsenceRepository.Verify(repo => repo.GetById(1), Times.Once);
            _mockAbsenceRepository.Verify(repo => repo.Delete(absence), Times.Once);
        }
    }
}