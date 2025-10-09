using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;
using AgendaUni.Services;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgendaUni.Common;
using System.Linq;

namespace AgendaUni.Tests
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
        public async Task AddClassAsync_ShouldReturnSuccess_WhenClassIsValid()
        {
            // Arrange
            var newClass = new Class { ClassName = "Math", MaximumAbsences = 3 };
            _mockClassRepository.Setup(repo => repo.AddAsync(newClass)).ReturnsAsync(newClass);

            // Act
            var result = await _classService.AddClassAsync(newClass);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Aula registrada com sucesso.", result.Message);
            _mockClassRepository.Verify(repo => repo.AddAsync(newClass), Times.Once);
        }

        [Fact]
        public async Task AddClassAsync_ShouldReturnFailure_WhenClassNameIsEmpty()
        {
            // Arrange
            var newClass = new Class { ClassName = "", MaximumAbsences = 3 };

            // Act
            var result = await _classService.AddClassAsync(newClass);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Informe o nome da aula.", result.Message);
            _mockClassRepository.Verify(repo => repo.AddAsync(It.IsAny<Class>()), Times.Never);
        }

        [Fact]
        public async Task AddClassAsync_ShouldReturnFailure_WhenMaximumAbsencesIsNegative()
        {
            // Arrange
            var newClass = new Class { ClassName = "Math", MaximumAbsences = -1 };

            // Act
            var result = await _classService.AddClassAsync(newClass);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Informe a quantidade de faltas.", result.Message);
            _mockClassRepository.Verify(repo => repo.AddAsync(It.IsAny<Class>()), Times.Never);
        }

        [Fact]
        public async Task UpdateClassAsync_ShouldReturnSuccess_WhenClassIsValid()
        {
            // Arrange
            var classToUpdate = new Class { Id = 1, ClassName = "Advanced Math", MaximumAbsences = 5 };

            // Act
            var result = await _classService.UpdateClassAsync(classToUpdate);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Aula atualizada com sucesso.", result.Message);
            _mockClassRepository.Verify(repo => repo.UpdateAsync(classToUpdate), Times.Once);
        }

        [Fact]
        public async Task UpdateClassAsync_ShouldReturnFailure_WhenClassNameIsEmpty()
        {
            // Arrange
            var classToUpdate = new Class { Id = 1, ClassName = "", MaximumAbsences = 5 };

            // Act
            var result = await _classService.UpdateClassAsync(classToUpdate);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("O nome da aula não pode ser vazio.", result.Message);
            _mockClassRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Class>()), Times.Never);
        }

        [Fact]
        public async Task UpdateClassAsync_ShouldReturnFailure_WhenMaximumAbsencesIsNegative()
        {
            // Arrange
            var classToUpdate = new Class { Id = 1, ClassName = "Advanced Math", MaximumAbsences = -1 };

            // Act
            var result = await _classService.UpdateClassAsync(classToUpdate);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("A quantidade máxima de faltas não pode ser negativa.", result.Message);
            _mockClassRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Class>()), Times.Never);
        }

        [Fact]
        public async Task DeleteClassAsync_ShouldReturnSuccess_WhenClassExists()
        {
            // Arrange
            var classId = 1;
            var classToDelete = new Class { Id = classId, ClassName = "Math", MaximumAbsences = 5 };
            _mockClassRepository.Setup(repo => repo.GetByIdAsync(classId)).ReturnsAsync(classToDelete);

            // Act
            var result = await _classService.DeleteClassAsync(classId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Aula deletada com sucesso.", result.Message);
            _mockClassRepository.Verify(repo => repo.DeleteAsync(classId), Times.Once);
        }

        [Fact]
        public async Task DeleteClassAsync_ShouldReturnFailure_WhenClassDoesNotExist()
        {
            // Arrange
            var classId = 99;
            _mockClassRepository.Setup(repo => repo.GetByIdAsync(classId)).ReturnsAsync((Class)null);

            // Act
            var result = await _classService.DeleteClassAsync(classId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Aula não encontrada.", result.Message);
            _mockClassRepository.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetAllClassesAsync_ShouldReturnAllClasses()
        {
            // Arrange
            var classes = new List<Class>
            {
                new Class { ClassName = "Math", MaximumAbsences = 5 },
                new Class { ClassName = "History", MaximumAbsences = 3 }
            };

            _mockClassRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(classes);

            // Act
            var result = await _classService.GetAllClassesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockClassRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetClassByIdAsync_ShouldReturnClass()
        {
            // Arrange
            var expectedClass = new Class { Id = 1, ClassName = "Math", MaximumAbsences = 5 };
            _mockClassRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(expectedClass);

            // Act
            var result = await _classService.GetClassByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Math", result.ClassName);
            Assert.Equal(5, result.MaximumAbsences);
            _mockClassRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }
    }
}
