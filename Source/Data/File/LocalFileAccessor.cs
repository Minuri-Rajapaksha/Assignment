using Data.File.Interfaces;
using Microsoft.Extensions.Configuration;
using Shared.Constants;
using Shared.Model.WebClientModel;
using System;
using System.IO;
using System.Net.Http.Headers;
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

        public async Task<FileUploadModel> UploadFileAsync(Stream stream, string fileName)
        {
            try
            {
                string newPath = Path.Combine(_configuration.GetValue<string>(AppSettings.UploadFilePath));
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                string fullPath = Path.Combine(newPath, fileName);
                stream.Position = 0;
                using (var fileStream = new FileStream(fullPath, FileMode.CreateNew))
                {
                    await stream.CopyToAsync(fileStream);
                    fileStream.Close();
                }

                return new FileUploadModel
                {
                    FilePath = fullPath,
                    Extension = Path.GetExtension(fullPath)
                };
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
    }
}
