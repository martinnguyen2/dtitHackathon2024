using System;

namespace Backend.Common.Models
{
    public class FileUploadResponse
    {
        public required string Text { get; set; }
        public required StatusEnum Status { get; set; }

        public enum StatusEnum
        {
            Error,
            Ok
        }
    }
}
