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
        /// <summary>
        /// Checks if a file exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Appends text to a file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public async Task AppendAllTextAsync(string path, string contents)
        {
            await File.AppendAllTextAsync(path, contents);
        }

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="path"></param>
        public void Delete(string path)
        {
            File.Delete(path);
        }

        /// <summary>
        /// Reads all lines from a file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IEnumerable<string> ReadLines(string path)
        {
            return File.ReadLines(path);
        }
    }
}
