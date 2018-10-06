using Data.Interfaces.File;
using Microsoft.Extensions.Configuration;
using Shared.Constants;
using Shared.Model.WebClientModel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Data.File
{
    public class LocalFileAccessor : IFileAccessor
    {
        private readonly IConfiguration _configuration;

        public LocalFileAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> UploadFileAsync(Stream stream, string fileName, string fileType)
        {
            var folderPath = Path.Combine(_configuration.GetValue<string>(AppSettings.UploadFilePath));
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fullPath = Path.Combine(folderPath, fileName);
            stream.Position = 0;
            using (var fileStream = new FileStream(fullPath, FileMode.CreateNew))
            {
                await stream.CopyToAsync(fileStream);
                fileStream.Close();
            }

            return true;
        }

        public string[] ReadTextFile(string filePath)
        {            
            return System.IO.File.ReadAllLines(filePath);
        }

        public FileInfo ReadExcelFile(string filePath)
        {
            return new FileInfo(filePath);
        }
    }
}
