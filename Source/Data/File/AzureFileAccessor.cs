using Data.Interfaces.File;
using Microsoft.Extensions.Configuration;
using Shared.Constants;
using Shared.Model.ServerModel;
using Shared.Model.WebClientModel;
using Shared.Queue;
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

        public Task<bool> UploadFileAsync(Stream stream, FileUploadModel fileUploadModel)
        {
            throw new NotImplementedException();
        }
    }
}

