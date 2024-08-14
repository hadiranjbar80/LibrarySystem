using System.Text.RegularExpressions;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common
{
    public class CommonMethods : ICommonMethods
    {
        public bool DeleteImage(string imageName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", imageName);
            if (File.Exists(filePath)) 
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

        public string GenerateCode(int startIndex = 0, int endIndex = 5)
        {
            var code = Regex.Replace(Guid.NewGuid().ToString().Replace("-", ""), "[A-Za-z ]", "");
            return code.AsSpan(startIndex, endIndex).ToString();
        }

        public string SaveImage(IFormFile file, string code)
        {
            if (file.Length > 0)
            {
                var imageName = $"{code}.{file.ContentType.Split("/").Last()}";
                
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", imageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return imageName;
            }

            return null;
        }
    }
}