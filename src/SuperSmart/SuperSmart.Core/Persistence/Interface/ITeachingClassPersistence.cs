using SuperSmart.Core.Data.ViewModels;

namespace SuperSmart.Core.Persistence.Interface
{
    public interface ITeachingClassPersistence
    {
        /// <summary>
        /// Creates a new teaching class for the given model if valid
        /// </summary>
        /// <param name="createTeachingClassViewModel"></param>
        /// <param name="loginToken"></param>
        void Create(CreateTeachingClassViewModel createTeachingClassViewModel, string loginToken);

        /// <summary>
        /// Joins the given class (referral) with the given account
        /// </summary>
        /// <param name="referral"></param>
        /// <param name="loginToken"></param>
        void Join(string referral, string loginToken);

        /// <summary>
        /// Remove a user from a teaching class
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="userId"></param>
        /// <param name="loginToken"></param>
        void RemoveUser(RemoveUserFromTeachingClassViewModel removeUserFromTeachingClassViewModel, string loginToken);

        /// <summary>
        /// Delete a existing teaching class with id
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
    }
}
