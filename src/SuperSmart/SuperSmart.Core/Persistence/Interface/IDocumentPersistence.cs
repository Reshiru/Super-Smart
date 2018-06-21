using SuperSmart.Core.Data.ViewModels;
using System;

namespace SuperSmart.Core.Persistence.Interface
{
    /// <summary>
    /// The document persistence to manage 
    /// document data
    /// </summary>
    public interface IDocumentPersistence
    {
        /// <summary>
        /// Sets an existing document for a given task on inactive
        /// </summary>
        /// <param name="createTaskViewModel"></param>
        /// <param name="loginToken"></param>
        void Create(CreateDocumentViewModel createDocumentViewModel, string loginToken);

        /// <summary>
        /// Sets an existing document for a given task on inactive
        /// </summary>
        /// <param name="createTaskViewModel"></param>
        /// <param name="loginToken"></param>
        void Delete(int id, string loginToken);

        /// <summary>
        /// Get Document Overview for a given task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="loginToken"></param>
        OverviewDocumentViewModel GetOverview(Int64 taskId, string loginToken);


        /// <summary>
        /// Get Document
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="loginToken"></param>
        DocumentViewModel GetDocument(Int64 documentId, string loginToken);
    }
}