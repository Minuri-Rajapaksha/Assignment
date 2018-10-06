using Data.Interfaces.File;
using Microsoft.Extensions.Configuration;
using Shared.Constants;
using Shared.Model.WebClientModel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Data.File
{
    public class AzureFileAccessor : IFileAccessor
    {
        private readonly IConfiguration _configuration;

        public AzureFileAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public FileInfo ReadExcelFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public string[] ReadTextFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UploadFileAsync(Stream stream, string fileName, string fileType)
        {
            throw new NotImplementedException();
        }
    }
}

