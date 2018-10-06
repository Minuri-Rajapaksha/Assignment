using Data.Interfaces.File;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OfficeOpenXml;
using Shared.Constants;
using Shared.Model.ServerModel;
using System;
using System.Collections.Generic;
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

        public async Task<string[]> ReadFileAsync(FileUploadModel fileUploadModel)
        {
            var directory = await GetBlobDirectoryAsync(fileUploadModel.FileType);
            var blockBlob = directory.GetBlockBlobReference(fileUploadModel.FileName);

            using (var stream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(stream);

                if (fileUploadModel.Extension == ".txt")
                {
                    StreamReader reader = new StreamReader(stream);
                    string text = reader.ReadToEnd();
                    return text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                }
                else
                {
                    List<string> rowArray = new List<string>();
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        int rowCount = worksheet.Dimension.Rows;
                        int ColCount = worksheet.Dimension.Columns;

                        for (int row = 1; row <= rowCount; row++)
                        {
                            var rawText = string.Empty;
                            for (int col = 1; col <= ColCount; col++)
                            {
                                rawText += worksheet.Cells[row, col].Value.ToString() + "\t";
                            }
                            rowArray.Add(rawText);
                        }
                        return rowArray.ToArray();
                    }
                }
            }
        }

        public async Task<bool> WriteFileAsync(Stream stream, FileUploadModel fileUploadModel)
        {
            var directory = await GetBlobDirectoryAsync(fileUploadModel.FileType);
            using (var binRead = new BinaryReader(stream))
            {
                CloudBlockBlob blob = directory.GetBlockBlobReference(fileUploadModel.FileName);
                stream.Position = 0;
                await blob.UploadFromStreamAsync(stream);
            }

            return true;
        }

        private async Task<CloudBlobDirectory> GetBlobDirectoryAsync(Shared.Enum.FileType type)
        {
            var storageBlobClient = CloudStorageAccount.Parse(_configuration.GetConnectionString(ConnectionStrings.BlobStorage))
                .CreateCloudBlobClient();
            var container = storageBlobClient.GetContainerReference(_configuration.GetValue<string>(AppSettings.ContainerName));
            await container.CreateIfNotExistsAsync();

            var directory = storageBlobClient.GetContainerReference(_configuration.GetValue<string>(AppSettings.ContainerName))
                .GetDirectoryReference(type.ToString());
            if (directory == null)
            {
                throw new IOException("Directory not found");
            }

            return directory;
        }
    }
}

