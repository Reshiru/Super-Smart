using SuperSmart.Core.Data.ViewModels;
using System;
using System.Collections.Generic;

namespace SuperSmart.Core.Persistence.Interface
{
    /// <summary>
    /// The subject persistence to manage subject
    /// data
    /// </summary>
    public interface ISubjectPersistence
    {
        /// <summary>
        /// Creates a new subject for a given teaching class
        /// </summary>
        /// <param name="createSubjectViewModel"></param>
        /// <param name="loginToken"></param>
        void Create(CreateSubjectViewModel createSubjectViewModel, string loginToken);

        /// <summary>
        /// Changes properties from a given subject class
        /// </summary>
        /// <param name="manageSubjectViewModel"></param>
        /// <param name="loginToken"></param>
        void Manage(ManageSubjectViewModel manageSubjectViewModel, string loginToken);
             
        /// <summary>
        /// Get Overview of subjects
        /// </summary>
        /// <param name="loginToken"></param>
        /// <param name="classId"></param>
        OverviewSubjectViewModel GetOverview(string loginToken, Int64 classId);

        /// <summary>
        /// Check if account has rights to manage subject
        /// </summary>
        /// <param name="subjectId"></param>
        bool IsAccountClassAdminOfSubject(Int64 subjectId, string loginToken);
    }
}