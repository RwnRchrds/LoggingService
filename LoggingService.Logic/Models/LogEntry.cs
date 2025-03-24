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
        public LogEntry(int id, DateTime date, string body)
        {
            Id = id;
            Date = date;
            Body = body;
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }

        [StringLength(255)]
        public string Body { get; set; }
    }
}
