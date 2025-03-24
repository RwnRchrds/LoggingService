using LoggingService.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingService.Logic.Interfaces
{
    public interface ILogService
    {
        /// <summary>
        /// Writes a log entry to the log.
        /// </summary>
        /// <param name="logEntry"></param>
        /// <returns></returns>
        Task WriteLogAsync(LogEntry logEntry);

        /// <summary>
        /// Gets a log entry by its unique identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LogEntry GetLog(int id);

        /// <summary>
        /// Gets all log entries.
        /// </summary>
        /// <returns></returns>
        IEnumerable<LogEntry> GetLogs();

        /// <summary>
        /// Gets all log entries within a specified date range.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        IEnumerable<LogEntry> GetLogs(DateTime from, DateTime to);

        /// <summary>
        /// Clears all log entries.
        /// </summary>
        /// <returns></returns>
        Task ClearLogs();
    }
}
