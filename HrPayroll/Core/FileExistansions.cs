using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Core
{
    public static class FileExistansions
    {
        public static bool IsFilePhotoFormat(this IFormFile Image)
        {
            return Image.ContentType == "image/jpeg" || Image.ContentType == "image/png";
        }

        public static string GetFileFormat(this IFormFile Image)
        {
            return Image.FileName.Substring(Image.FileName.LastIndexOf(".") + 1, 3);
        }

        public static async Task SaveFileAsync(this IFormFile Image, string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await Image.CopyToAsync(stream);
            }
        }
    }
}
