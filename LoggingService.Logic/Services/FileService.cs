using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoggingService.Logic.Interfaces;

namespace LoggingService.Logic.Services
{
    public class FileService : IFileService
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public async Task AppendAllTextAsync(string path, string contents)
        {
            await File.AppendAllTextAsync(path, contents);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        public IEnumerable<string> ReadLines(string path)
        {
            return File.ReadLines(path);
        }
    }
}
