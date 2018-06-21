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
        /// Get subject to manage
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="loginToken"></param>
        ManageSubjectViewModel GetManagedSubject(Int64 subjectId, string loginToken);

        /// <summary>
        /// Changes properties from a given subject class
        /// </summary>
        /// <param name="manageSubjectViewModel"></param>
        /// <param name="loginToken"></param>
        Int64 Manage(ManageSubjectViewModel manageSubjectViewModel, string loginToken);

        /// <summary>
        /// Get Overview of subjects
        /// </summary>
        /// <param name="loginToken"></param>
        /// <param name="classId"></param>
        OverviewSubjectViewModel GetOverview(string loginToken, Int64 classId);
    }
}