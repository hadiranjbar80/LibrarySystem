using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface ICommonMethods
    {
        /// <summary>
        /// This method gets two parameters and returns a substring of the generated code between start and end index.
        /// </summary>
        /// <param name="endIndex"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        string GenerateCode(int startIndex = 0, int endIndex = 5);

        /// <summary>
        /// This method gets a file () and saves it into local directory
        /// </summary>
        /// <param name="file">The that is supose to be stored in local directory</param>
        /// <param name="code">A random unique code that will be used as filename</param>
        /// <returns></returns>
        string SaveImage(IFormFile file, string code);

        bool DeleteImage(string imageName);
    }
}