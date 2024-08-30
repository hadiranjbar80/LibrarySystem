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
        /// <returns>A string that is the generated code.</returns>
        string GenerateCode(int startIndex = 0, int endIndex = 5);

        /// <summary>
        /// This method gets a file () and saves it into local directory
        /// </summary>
        /// <param name="file">The that is supose to be stored in local directory</param>
        /// <param name="code">A random unique code that will be used as filename</param>
        /// <returns>A string which represents the name of the saved image</returns>
        string SaveImage(IFormFile file, string code);

        /// <summary>
        /// Gets an image name and deletes the image from local storage
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns>A boolean type which represents if the image in is being deleted or not</returns>
        bool DeleteImage(string imageName);

        /// <summary>
        /// Reads a template from project source
        /// </summary>
        /// <param name="link">Any link that should be included in the template</param>
        /// <param name="title">Title of the email template</param>
        /// <param name="templateName">Specifies which template should be read from storage</param>
        /// <returns>A string which represents an email layout</returns>
        string EmailTeplateReader(string title, string templateName, string link = null, string password = null);
    }
}