using LoggingService.Logic.Exceptions;
using LoggingService.Logic.Interfaces;
using LoggingService.Logic.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace LoggingService.Logic.Services
{
    public sealed class TextFileLogService : ILogService
    {
        private readonly IFileService _fileService;
        private readonly string _logFilePath;
        private static readonly SemaphoreSlim Semaphore = new(1, 1);
        private readonly ConcurrentDictionary<int, LogEntry> _logCache = new();

        public TextFileLogService(IFileService fileService, string logFilePath)
        {
            _fileService = fileService;
            _logFilePath = logFilePath;
            LoadCache();
        }

        /// <summary>
        /// Writes a log entry to the log file.
        /// </summary>
        /// <param name="logEntry"></param>
        /// <returns></returns>
        /// <exception cref="InvalidRequestException"></exception>
        public async Task WriteLogAsync(LogEntry logEntry)
        {
            await Semaphore.WaitAsync();
            try
            {
                if (_logCache.ContainsKey(logEntry.Id))
                {
                    throw new InvalidRequestException($"A log with id {logEntry.Id} already exists.");
                }

                string logMessage = JsonConvert.SerializeObject(logEntry) + Environment.NewLine;
                await _fileService.AppendAllTextAsync(_logFilePath, logMessage);

                _logCache[logEntry.Id] = logEntry; // Update cache
            }
            finally
            {
                Semaphore.Release();
            }
        }

        /// <summary>
        /// Retrieves a log entry by its unique identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public LogEntry GetLog(int id)
        {
            if (_logCache.TryGetValue(id, out var log))
            {
                return log;
            }

            throw new NotFoundException($"Log with id {id} not found.");
        }

        /// <summary>
        /// Retrieves all log entries.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LogEntry> GetLogs()
        {
            return _logCache.Values.OrderByDescending(l => l.Date);
        }

        /// <summary>
        /// Retrieves all log entries within a specified date range.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IEnumerable<LogEntry> GetLogs(DateTime from, DateTime to)
        {
            var filteredLogs = _logCache.Values
                .Where(l => l.Date >= from && l.Date <= to)
                .OrderByDescending(l => l.Date);
            
            return filteredLogs;
        }

        /// <summary>
        /// Clears all log entries.
        /// </summary>
        /// <returns></returns>
        public async Task ClearLogs()
        {
            await Semaphore.WaitAsync();
            try
            {
                if (_fileService.Exists(_logFilePath))
                {
                    _fileService.Delete(_logFilePath);
                }
                _logCache.Clear();
            }
            finally
            {
                Semaphore.Release();
            }
        }

        private void LoadCache()
        {
            if (!_fileService.Exists(_logFilePath)) return;

            foreach (var line in _fileService.ReadLines(_logFilePath))
            {
                try
                {
                    var entry = JsonConvert.DeserializeObject<LogEntry>(line);
                    if (entry != null)
                    {
                        _logCache[entry.Id] = entry;
                    }
                }
                catch (JsonException)
                {
                    // Skip malformed lines
                }
            }
        }
    }
}
