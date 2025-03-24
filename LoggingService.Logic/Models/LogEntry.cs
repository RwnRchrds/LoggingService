using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingService.Logic.Models
{
    public class LogEntry
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <param name="body"></param>
        public LogEntry(int id, DateTime date, string body)
        {
            Id = id;
            Date = date;
            Body = body;
        }

        /// <summary>
        /// The unique identifier for the log entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The date and time the log entry was created.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The body of the log entry.
        /// </summary>
        [StringLength(255)]
        public string Body { get; set; }
    }
}
