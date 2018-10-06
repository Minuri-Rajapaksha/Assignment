using Shared.Model.WebClientModel;
using System.IO;
using System.Threading.Tasks;

namespace Data.Interfaces.File
{
    public interface IFileAccessor
    {
        Task<FileUploadModel> UploadFileAsync(Stream stream, string fileName);
    }
}
