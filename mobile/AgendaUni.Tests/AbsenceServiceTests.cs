using AgendaUni.Models;
using AgendaUni.Services;
using AgendaUni.Repositories.Interfaces;
using AgendaUni.Common;
using Moq;
using Xunit;

namespace AgendaUni.Tests;

public class AbsenceServiceTests
{
    private readonly Mock<IAbsenceRepository> _absenceRepositoryMock;
    private readonly AbsenceService _absenceService;

    public AbsenceServiceTests()
    {
        _absenceRepositoryMock = new Mock<IAbsenceRepository>();
        _absenceService = new AbsenceService(_absenceRepositoryMock.Object);
    }

    [Fact]
    public async Task RegisterAbsenceAsync_ShouldReturnFailure_WhenClassIdIsZero()
    {
        // Arrange
        var absence = new Absence
        {
            ClassId = 0,
            AbsenceReason = "Sick",
            AbsenceDate = DateTime.Now
        };

        // Act
        var result = await _absenceService.RegisterAbsenceAsync(absence);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Selecione uma aula.", result.Message);
        _absenceRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Absence>()), Times.Never);
    }

    [Fact]
    public async Task RegisterAbsenceAsync_ShouldReturnFailure_WhenReasonIsEmpty()
    {
        // Arrange
        var absence = new Absence
        {
            ClassId = 1,
            AbsenceReason = "",
            AbsenceDate = DateTime.Now
        };

        // Act
        var result = await _absenceService.RegisterAbsenceAsync(absence);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Informe o motivo da falta.", result.Message);
        _absenceRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Absence>()), Times.Never);
    }

    [Fact]
    public async Task RegisterAbsenceAsync_ShouldReturnFailure_WhenReasonIsWhitespace()
    {
        // Arrange
        var absence = new Absence
        {
            ClassId = 1,
            AbsenceReason = "   ",
            AbsenceDate = DateTime.Now
        };

        // Act
        var result = await _absenceService.RegisterAbsenceAsync(absence);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Informe o motivo da falta.", result.Message);
        _absenceRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Absence>()), Times.Never);
    }

    [Fact]
    public async Task RegisterAbsenceAsync_ShouldAddAbsence_WhenValidData()
    {
        // Arrange
        var absence = new Absence
        {
            ClassId = 1,
            AbsenceReason = "Trip",
            AbsenceDate = DateTime.Now
        };

        // Act
        var result = await _absenceService.RegisterAbsenceAsync(absence);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Falta registrada com sucesso.", result.Message);
        _absenceRepositoryMock.Verify(r => r.AddAsync(absence), Times.Once);
    }
}
