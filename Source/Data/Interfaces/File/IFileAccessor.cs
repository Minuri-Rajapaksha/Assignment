using Shared.Model.ServerModel;
using System.IO;
using System.Threading.Tasks;

namespace Data.Interfaces.File
{
    public interface IFileAccessor
    {
        Task<bool> WriteFileAsync(Stream stream, FileUploadModel fileUploadModel);

        Task<string[]> ReadFileAsync(FileUploadModel fileUploadModel);
    }
}
