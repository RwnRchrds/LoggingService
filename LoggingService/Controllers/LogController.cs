using System.Net;
using LoggingService.Logic.Interfaces;
using LoggingService.Logic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }   

        
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddLog([FromBody] LogEntry logEntry)
        {
            await _logService.WriteLogAsync(logEntry);

            return Ok("Log added successfully.");
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetLog([FromRoute] int id)
        {
            var log = _logService.GetLog(id);
            return Ok(log);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAllLogs([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            IEnumerable<LogEntry> logs;

            if (!dateFrom.HasValue && !dateTo.HasValue)
            {
                logs = _logService.GetLogs();
            }
            else if (!dateFrom.HasValue || !dateTo.HasValue)
            {
                return StatusCode((int)HttpStatusCode.BadRequest,
                    "You must provide both a 'to' and 'from' date to perform date filtering.");
            }
            else
            {
                logs = _logService.GetLogs(dateFrom.Value, dateTo.Value);
            } 
            
            return Ok(logs);
        }

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> ClearLogs()
        {
            await _logService.ClearLogs();
            return Ok("Logs cleared successfully.");
        }
    }
}
