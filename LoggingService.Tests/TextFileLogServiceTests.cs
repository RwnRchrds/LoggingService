using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoggingService.Logic.Exceptions;
using LoggingService.Logic.Interfaces;
using LoggingService.Logic.Models;
using LoggingService.Logic.Services;
using Newtonsoft.Json;

namespace LoggingService.Tests
{
    [TestFixture]
    public class TextFileLogServiceTests
    {
        private Mock<IFileService> _fileMock;
        private TextFileLogService _logService;
        private string _testFilePath = "test_logs.txt";

        [SetUp]
        public void SetUp()
        {
            _fileMock = new Mock<IFileService>();

            _fileMock.Setup(f => f.Exists(_testFilePath)).Returns(true);
            _fileMock.Setup(f => f.ReadLines(_testFilePath)).Returns(new[]
            {
                JsonConvert.SerializeObject(new LogEntry(1, DateTime.UtcNow, "Test Log 1")),
                JsonConvert.SerializeObject(new LogEntry(2, DateTime.UtcNow.AddMinutes(-10), "Test Log 2"))
            });

            _logService = new TextFileLogService(_fileMock.Object, _testFilePath);
        }

        #region WriteLogAsync Tests

        [Test]
        public async Task WriteLogAsync_ShouldWriteLog_WhenLogDoesNotExist()
        {
            // Arrange
            var newLog = new LogEntry(3, DateTime.UtcNow, "New Log");
            _fileMock.Setup(f => f.AppendAllTextAsync(_testFilePath, It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            await _logService.WriteLogAsync(newLog);

            // Assert
            var retrievedLog = _logService.GetLog(3);
            Assert.That(retrievedLog, Is.Not.Null);
            Assert.That(retrievedLog.Id, Is.EqualTo(3));
            Assert.That(retrievedLog.Body, Is.EqualTo("New Log"));
            _fileMock.Verify(f => f.AppendAllTextAsync(_testFilePath, It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void WriteLogAsync_ShouldThrowException_WhenLogAlreadyExists()
        {
            // Arrange
            var duplicateLog = new LogEntry(1, DateTime.UtcNow, "Duplicate Log");

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidRequestException>(() => _logService.WriteLogAsync(duplicateLog));
            Assert.That(ex!.Message, Is.EqualTo("A log with id 1 already exists."));
        }

        #endregion

        #region GetLog Tests

        [Test]
        public void GetLog_ShouldReturnCorrectLog_WhenLogExists()
        {
            // Act
            var log = _logService.GetLog(1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(log, Is.Not.Null);
                Assert.That(log.Id, Is.EqualTo(1));
                Assert.That(log.Body, Is.EqualTo("Test Log 1"));
            });
        }

        [Test]
        public void GetLog_ShouldThrowException_WhenLogDoesNotExist()
        {
            // Act & Assert
            var ex = Assert.Throws<NotFoundException>(() => _logService.GetLog(99));
            Assert.That(ex!.Message, Is.EqualTo("Log with id 99 not found."));
        }

        #endregion

        #region GetLogs Tests

        [Test]
        public void GetLogs_ShouldReturnLogsOrderedByDate()
        {
            // Act
            var logs = _logService.GetLogs().ToList();

            // Assert
            Assert.That(logs.Count(), Is.EqualTo(2));
            Assert.That(logs.First().Id, Is.EqualTo(1));
        }

        [Test]
        public void GetLogs_ShouldReturnLogsInDateRange()
        {
            // Arrange
            var from = DateTime.UtcNow.AddMinutes(-15);
            var to = DateTime.UtcNow;

            // Act
            var logs = _logService.GetLogs(from, to).ToList();

            // Assert
            Assert.That(logs.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetLogs_ShouldReturnEmpty_WhenNoLogsInDateRange()
        {
            // Arrange
            var from = DateTime.UtcNow.AddDays(-10);
            var to = DateTime.UtcNow.AddDays(-5);

            // Act
            var logs = _logService.GetLogs(from, to);

            // Assert
            Assert.That(logs, Is.Empty);
        }

        #endregion

        #region ClearLogs Tests

        [Test]
        public async Task ClearLogs_ShouldDeleteFileAndClearCache()
        {
            // Arrange
            _fileMock.Setup(f => f.Exists(_testFilePath)).Returns(true);
            _fileMock.Setup(f => f.Delete(_testFilePath));

            // Act
            await _logService.ClearLogs();

            // Assert
            Assert.That(_logService.GetLogs(), Is.Empty);
            _fileMock.Verify(f => f.Delete(_testFilePath), Times.Once);
        }

        #endregion
    }
}
