using Microsoft.AspNetCore.Http;
using Shared.Model.WebClientModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.File.Interfaces
{
    public interface IFileAccessor
    {
        Task<FileUploadModel> UploadFile(IFormFile file);
    }
}
