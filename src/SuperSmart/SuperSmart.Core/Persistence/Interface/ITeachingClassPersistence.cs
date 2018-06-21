using SuperSmart.Core.Data.ViewModels;
using System;

namespace SuperSmart.Core.Persistence.Interface
{
    /// <summary>
    /// The teaching class persistence to manage teaching classes
    /// </summary>
    public interface ITeachingClassPersistence
    {
        /// <summary>
        /// Creates a new teaching class for the given model if valid
        /// </summary>
        /// <param name="createTeachingClassViewModel"></param>
        /// <param name="loginToken"></param>
        void Create(CreateTeachingClassViewModel createTeachingClassViewModel, string loginToken);

        /// <summary>
        /// Get teaching class to manage
        /// </summary>
        /// <param name="teachingClassId"></param>
        /// <param name="loginToken"></param>
        ManageTeachingClassViewModel GetManagedTeachingClass(Int64 teachingClassId, string loginToken);

        /// <summary>
        /// Changes properties from a given teaching class
        /// </summary>
        /// <param name="manageTeachingClassViewModel"></param>
        /// <param name="loginToken"></param>
        void Manage(ManageTeachingClassViewModel manageTeachingClassViewModel, string loginToken);

        /// <summary>
        /// Joins the given class (referral) with the given account
        /// </summary>
        /// <param name="referral"></param>
        /// <param name="loginToken"></param>
        void Join(string referral, string loginToken);

        /// <summary>
        /// Remove a user from a teaching class
        /// </summary>
        /// <param name="removeUserFromTeachingClassViewModel"></param>
        /// <param name="loginToken"></param>
        void RemoveUser(RemoveUserFromTeachingClassViewModel removeUserFromTeachingClassViewModel, string loginToken);

        /// <summary>
        /// Delete a existing teaching class with id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginToken"></param>
        void Delete(int id, string loginToken);

        /// <summary>
        /// Changes the referral from the given teaching class id
        /// </summary>
        /// <param name="id">teaching class id</param>
        /// <param name="loginToken"></param>
        void ChangeReferral(int id, string loginToken);

        /// <summary>
        /// Get teachingClasses for user by loginToken
        /// </summary>
        /// <param name="loginToken"></param>
        OverviewTeachingClassViewModel GetOverview(string loginToken);

        /// <summary>
        /// Get all accounts from a teaching class
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="loginToken"></param>
        OverviewStudentsViewModel GetStudents(Int64 classId, string loginToken);
    }
}