using Shared.Model.WebClientModel;
using System.IO;
using System.Threading.Tasks;

namespace Data.File.Interfaces
{
    public interface IFileAccessor
    {
        Task<FileUploadModel> UploadFileAsync(Stream stream, string fileName);
    }
}
