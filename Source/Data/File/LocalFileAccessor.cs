using Data.Interfaces.File;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using Shared.Constants;
using Shared.Model.ServerModel;
using Shared.Model.WebClientModel;
using Shared.Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Data.File
{
    public class LocalFileAccessor : IFileAccessor
    {
        private readonly IConfiguration _configuration;

        public LocalFileAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> UploadFileAsync(Stream stream, FileUploadModel fileUploadModel)
        {
            var folderPath = Path.Combine(_configuration.GetValue<string>(AppSettings.FolderPath), fileUploadModel.FileType.ToString());
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fullPath = Path.Combine(folderPath, fileUploadModel.FileName);
            stream.Position = 0;
            using (var fileStream = new FileStream(fullPath, FileMode.CreateNew))
            {
                await stream.CopyToAsync(fileStream);
                fileStream.Close();
            }

            return true;
        }

        public async Task<string[]> ReadFileAsync(FileUploadModel fileUploadModel)
        {
            var folderPath = Path.Combine(_configuration.GetValue<string>(AppSettings.FolderPath), fileUploadModel.FileType.ToString());
            var fullPath = Path.Combine(folderPath, fileUploadModel.FileName);

            if (fileUploadModel.Extension == ".txt")
            {
                return System.IO.File.ReadAllLines(fullPath);
            }
            else
            {
                List<string> rowArray = new List<string>();
                using (ExcelPackage package = new ExcelPackage(new FileInfo(fullPath)))
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
}
