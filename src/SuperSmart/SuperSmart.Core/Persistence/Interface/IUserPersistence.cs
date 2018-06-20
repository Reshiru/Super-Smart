using SuperSmart.Core.Data.ViewModels;

namespace SuperSmart.Core.Persistence.Interface
{
    /// <summary>
    /// The user persistence to manage user data
    /// </summary>
    public interface IUserPersistence
    {
        /// <summary>
        /// Get user by loginToken
        /// </summary>
        /// <param name="loginToken"></param>
        UserViewModel GetUserByLoginToken(string loginToken);

        /// <summary>
        /// Get full name by loginToken
        /// </summary>
        /// <param name="loginToken"></param>
        UserNameViewModel GetFullNameFromUser(string loginToken);
    }
}
