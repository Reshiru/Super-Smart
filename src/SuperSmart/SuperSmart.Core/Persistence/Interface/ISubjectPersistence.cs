using SuperSmart.Core.Data.ViewModels;
using System.Collections.Generic;

namespace SuperSmart.Core.Persistence.Interface
{
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
        /// Get subjects for a overview by Id of a teaching class
        /// </summary>
        /// <param name="classId"></param>
        List<OverviewSubjectViewModel> GetSubjectsForOverviewByClassId(long classId);
    }
}
