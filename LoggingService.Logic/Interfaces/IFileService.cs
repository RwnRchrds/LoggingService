using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingService.Logic.Interfaces
{
    public interface IFileService
    {
        /// <summary>
        /// Check if a file exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool Exists(string path);

        /// <summary>
        /// Read all text from a file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        Task AppendAllTextAsync(string path, string contents);

        /// <summary>
        /// Write all text to a file.
        /// </summary>
        /// <param name="path"></param>
        void Delete(string path);
        IEnumerable<string> ReadLines(string path);
    }
}
