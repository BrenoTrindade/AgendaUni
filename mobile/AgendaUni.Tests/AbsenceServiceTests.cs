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
    public async Task AddAbsenceAsync_ShouldReturnFailure_WhenClassIdIsZero()
    {
        // Arrange
        var absence = new Absence { ClassId = 0, AbsenceReason = "Sick", AbsenceDate = DateTime.Now };

        // Act
        var result = await _absenceService.AddAbsenceAsync(absence);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Selecione uma aula.", result.Message);
        _absenceRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Absence>()), Times.Never);
    }

    [Fact]
    public async Task AddAbsenceAsync_ShouldReturnFailure_WhenReasonIsEmpty()
    {
        // Arrange
        var absence = new Absence { ClassId = 1, AbsenceReason = "", AbsenceDate = DateTime.Now };

        // Act
        var result = await _absenceService.AddAbsenceAsync(absence);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Informe o motivo da falta.", result.Message);
        _absenceRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Absence>()), Times.Never);
    }

    [Fact]
    public async Task AddAbsenceAsync_ShouldReturnSuccess_WhenValidData()
    {
        // Arrange
        var absence = new Absence { ClassId = 1, AbsenceReason = "Trip", AbsenceDate = DateTime.Now };

        // Act
        var result = await _absenceService.AddAbsenceAsync(absence);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Falta registrada com sucesso.", result.Message);
        _absenceRepositoryMock.Verify(r => r.AddAsync(absence), Times.Once);
    }

    [Fact]
    public async Task UpdateAbsenceAsync_ShouldReturnSuccess_WhenValidData()
    {
        // Arrange
        var absence = new Absence { Id = 1, ClassId = 1, AbsenceReason = "Updated Reason", AbsenceDate = DateTime.Now };

        // Act
        var result = await _absenceService.UpdateAbsenceAsync(absence);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Falta atualizada com sucesso.", result.Message);
        _absenceRepositoryMock.Verify(r => r.UpdateAsync(absence), Times.Once);
    }

    [Fact]
    public async Task UpdateAbsenceAsync_ShouldReturnFailure_WhenClassIdIsZero()
    {
        // Arrange
        var absence = new Absence { Id = 1, ClassId = 0, AbsenceReason = "Updated Reason" };

        // Act
        var result = await _absenceService.UpdateAbsenceAsync(absence);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Selecione uma aula.", result.Message);
        _absenceRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Absence>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAbsenceAsync_ShouldReturnFailure_WhenReasonIsEmpty()
    {
        // Arrange
        var absence = new Absence { Id = 1, ClassId = 1, AbsenceReason = "" };

        // Act
        var result = await _absenceService.UpdateAbsenceAsync(absence);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Informe o motivo da falta.", result.Message);
        _absenceRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Absence>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAbsenceAsync_ShouldReturnSuccess_WhenAbsenceExists()
    {
        // Arrange
        var absenceId = 1;
        var existingAbsence = new Absence { Id = absenceId, ClassId = 1, AbsenceReason = "Test" };
        _absenceRepositoryMock.Setup(r => r.GetByIdAsync(absenceId)).ReturnsAsync(existingAbsence);

        // Act
        var result = await _absenceService.DeleteAbsenceAsync(absenceId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Falta deletada com sucesso.", result.Message);
        _absenceRepositoryMock.Verify(r => r.DeleteAsync(absenceId), Times.Once);
    }

    [Fact]
    public async Task DeleteAbsenceAsync_ShouldReturnFailure_WhenAbsenceDoesNotExist()
    {
        // Arrange
        var absenceId = 99;
        _absenceRepositoryMock.Setup(r => r.GetByIdAsync(absenceId)).ReturnsAsync((Absence)null);

        // Act
        var result = await _absenceService.DeleteAbsenceAsync(absenceId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Falta não encontrada.", result.Message);
        _absenceRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
