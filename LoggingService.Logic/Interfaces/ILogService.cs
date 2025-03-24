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
        Task WriteLogAsync(LogEntry logEntry);
        LogEntry GetLog(int id);
        IEnumerable<LogEntry> GetLogs();
        IEnumerable<LogEntry> GetLogs(DateTime from, DateTime to);
        Task ClearLogs();
    }
}
