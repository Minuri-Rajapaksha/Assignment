using Shared.Model.WebClientModel;
using System.IO;
using System.Threading.Tasks;

namespace Data.Interfaces.File
{
    public interface IFileAccessor
    {
        Task<bool> UploadFileAsync(Stream stream, string fileName, string fileType);

        string[] ReadTextFile(string filePath);

        FileInfo ReadExcelFile(string filePath);
    }
}
