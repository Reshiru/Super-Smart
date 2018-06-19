using SuperSmart.Core.Data.ViewModels;

namespace SuperSmart.Core.Persistence.Interface
{
    public interface IDocumentPersistence
    {
        void Create(CreateDocumentViewModel createDocumentViewModel, string loginToken);

        /// <summary>
        /// Sets an existing document for a given task on inactive
        /// </summary>
        /// <param name="createTaskViewModel"></param>
        /// <param name="loginToken"></param>
        void Delete(int id, string loginToken);
    }
}