using Shared.Model.ServerModel;
using Shared.Model.WebClientModel;
using Shared.Queue;
using System.IO;
using System.Threading.Tasks;

namespace Data.Interfaces.File
{
    public interface IFileAccessor
    {
        Task<bool> UploadFileAsync(Stream stream, FileUploadModel fileUploadModel);

        Task<string[]> ReadFileAsync(FileUploadModel fileUploadModel);
    }
}
