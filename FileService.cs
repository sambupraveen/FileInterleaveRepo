using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileTask
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _log;
        public FileService(ILogger<FileService> log)
        {
            _log = log;
        }

        public async Task Read()
        {
            try
            {
                string filePath1 = @"C:\Praveen\test.txt";
                string filePath2 = @"C:\Praveen\test123.txt";
                var output = await InterleaveFiles(filePath1, filePath2);
                Console.WriteLine(string.Join(" ", output));
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// This Function takes 2 parameters and Interleave the words from that files.
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        private async Task<List<string>> InterleaveFiles(string file1, string file2)
        {
            List<string> output = new List<string>();
            try
            {
                var file1Words = await ReadFile(file1);
                var file2Words = await ReadFile(file2);

                if (file1Words != null && file2Words != null)
                { 
                    foreach (var word in InterLeaveWordsService.InterLeaveWords(file1Words, file2Words))
                    {
                       output.Add(word);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return output;
        }

        private async Task<List<string>> ReadFile(string filePath)
        {
            if (filePath is null)
            {
                _log.LogError($"You did not supply a file path.");
                return null;
            }

            try
            {
                var content = await File.ReadAllTextAsync(filePath);
                var words = content.Split(' ').ToList();
                return words;
            }
            catch (FileNotFoundException)
            {
                _log.LogError("The file or directory cannot be found.");
            }
            catch (DirectoryNotFoundException)
            {
                _log.LogError("The file or directory cannot be found.");
            }
            catch (DriveNotFoundException)
            {
                _log.LogError("The drive specified in 'path' is invalid.");
            }
            catch (PathTooLongException)
            {
                _log.LogError("'path' exceeds the maxium supported path length.");
            }
            catch (UnauthorizedAccessException)
            {
                _log.LogError("You do not have permission to create this file.");
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
            {
                _log.LogError("There is a sharing violation.");
            }
            catch (IOException e)
            {
                _log.LogError($"An exception occurred:\nError code: " +
                                  $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
            }
            catch (Exception)
            {
                _log.LogError("There is an Error reading File.");

            }
            return null;
        }

    }
}
