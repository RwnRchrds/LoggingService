using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingService.Logic.Interfaces
{
    public interface IFileService
    {
        bool Exists(string path);
        Task AppendAllTextAsync(string path, string contents);
        void Delete(string path);
        IEnumerable<string> ReadLines(string path);
    }
}
