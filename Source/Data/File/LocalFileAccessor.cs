using Data.File.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shared.Constants;
using Shared.Model.WebClientModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
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

        public async Task<FileUploadModel> UploadFile(IFormFile file)
        {
            try
            {
                string newPath = Path.Combine(_configuration.GetValue<string>(AppSettings.UploadFilePath));
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    var result = new FileUploadModel
                    {
                        FilePath = fullPath,
                        Extension = Path.GetExtension(fullPath)
                    };
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
    }
}
