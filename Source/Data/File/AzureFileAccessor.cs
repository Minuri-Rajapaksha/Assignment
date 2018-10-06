using Data.Interfaces.File;
using Microsoft.Extensions.Configuration;
using Shared.Model.ServerModel;
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

        public Task<string[]> ReadFileAsync(FileUploadModel fileUploadModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WriteFileAsync(Stream stream, FileUploadModel fileUploadModel)
        {
            throw new NotImplementedException();
        }
    }
}

