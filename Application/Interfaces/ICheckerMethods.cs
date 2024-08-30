using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICheckerMethods
    {
        /// <summary>
        /// Gets user register code and checks if user has valid subscription 
        /// </summary>
        /// <param name="registerCode"></param>
        /// <returns>A boolean type that specifies user has subscription or not</returns>
        Task<bool> CheckUserSubscription(string registerCode);


        /// <summary>
        /// Gets book code and checks if a specific book is taken or not
        /// </summary>
        /// <param name="bookCode"></param>
        /// <returns>A boolean type that specifies book has already been taken or not</returns>
        Task<bool> CheckIfTheBookIsTaken(string bookCode);
    }
}
