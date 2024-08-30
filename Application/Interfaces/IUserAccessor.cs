using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserAccessor
    {

        /// <summary>
        /// Returns the id of the user
        /// </summary>
        /// <returns>A string which represents currently loged in user id</returns>
        string GetUserId();

        /// <summary>
        /// This method gets id of a user and returns subscription related to that user
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>Any subscription that is related to the given user id</returns>
        Task<Domain.Entities.Subscription> GetUserSubscription(string id);

        /// <summary>
        /// Returns user by id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>Any user that is related to the given id</returns>
        Task<AppUser> GetCurrentUser(string id);

        /// <summary>
        /// Resturns user by id 
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>Any user that is related to the given id (Included related subscriptions)</returns>
        Task<AppUser> GetCurrentUserSubscriptionIncluded(string id);

        /// <summary>
        /// Gets a specific user with the given code
        /// </summary>
        /// <param name="registerCode"></param>
        /// <returns></returns>
        Task<AppUser> GetUserByRegisterCode(string registerCode);

        /// <summary>
        /// Gets all users in the database
        /// </summary>
        /// <returns></returns>
        Task<List<AppUser>> GetAllUsers();

        /// <summary>
        /// Gets all borrowed books by the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A list of type Lending which represents user lendings</returns>
        Task<List<Domain.Entities.Lending>> GetUserLendings(string userId);
    }
}
