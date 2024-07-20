using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class HandleFile
    {
        public static async Task<string> Upload(string folder,IFormFile? file)
        {
            string fileName = "";
            if (file == null)
            {
                return null;
            }
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = DateTime.Now.Ticks.ToString() + extension + folder;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), folder);

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var exactPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);

                using (var stream = new FileStream(exactPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return fileName;
        }
        public static async Task<string> ReadFile(string folder, string? fileName)
        {
            string content = "";
            if (fileName == null)
            {
                return content;
            }     
            try
            {
                // Đường dẫn chính xác đến tệp
                var exactPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);

                if (File.Exists(exactPath))
                {
                    using (var reader = new StreamReader(exactPath))
                    {
                        content = await reader.ReadToEndAsync();
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return content;
        }
    }
}
