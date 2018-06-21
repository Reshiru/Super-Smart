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
        /// Get document to download
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="loginToken"></param>
        DownloadDocumentViewModel Download(Int64 documentId, string loginToken);

        /// <summary>
        /// Get document to manage
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="loginToken"></param>
        ManageDocumentViewModel GetManagedDocument(Int64 documentId, string loginToken);

        /// <summary>
        /// Manage document
        /// </summary>
        /// <param name="manageDocumentViewModel"></param>
        /// <param name="loginToken"></param>
        void Manage(ManageDocumentViewModel manageDocumentViewModel, string loginToken);
    }
}