using Backend.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Backend.Common.Utils
{
    public class UploadHandler
    {
        public static FileUploadResponse Upload(IFormFile file)
        {
            // Check valid extensions
            List<string> validExtensions = [ ".csv" ];
            string extension = Path.GetExtension(file.FileName);
            
            if (!validExtensions.Contains(extension))
            {
                return new FileUploadResponse
                {
                    Status = FileUploadResponse.StatusEnum.Error,
                    Text = $"Extension {extension} is not valid"
                };
            }

            // Check valid size
            long size = file.Length;

            // Set max size at 10MB
            if (size > (100 * 1024 * 1024))
            {
                return new FileUploadResponse
                {
                    Status = FileUploadResponse.StatusEnum.Error,
                    Text = "Maximum file size can be 100MB."
                };
            }

            // Check valid name
            if (file.FileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                return new FileUploadResponse
                {
                    Status = FileUploadResponse.StatusEnum.Error,
                    Text = "Invalid characters in file name."
                };
            }

            string path = Path.Combine(AppContext.BaseDirectory, "Data", file.FileName);

            if (File.Exists(Path.Combine(AppContext.BaseDirectory, "Data", file.FileName)))
            {
                bool isNewNameFound = false;
                for (int i = 0; i < 10000; i++)
                {
                    string name = Path.GetFileNameWithoutExtension(file.FileName);
                    string newName = name + i.ToString() + extension;

                    if (!File.Exists(Path.Combine(AppContext.BaseDirectory, "Data", newName)))
                    {
                        path = Path.Combine(AppContext.BaseDirectory, "Data", newName);
                        isNewNameFound = true;
                        break;
                    }
                }

                if (isNewNameFound == false)
                {
                    return new FileUploadResponse
                    {
                        Status = FileUploadResponse.StatusEnum.Error,
                        Text = "Could not find a suitable new name."
                    };
                }
            }

            FileStream fileStream = new FileStream(path, FileMode.Create);
            file.CopyTo(fileStream);
            fileStream.Dispose();
            fileStream.Close();

            return new FileUploadResponse
            {
                Status = FileUploadResponse.StatusEnum.Ok,
                Text = "File uploaded."
            };
        }
    }
}
