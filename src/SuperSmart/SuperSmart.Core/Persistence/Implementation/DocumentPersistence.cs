using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Helper;
using SuperSmart.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperSmart.Core.Persistence.Implementation
{
    /// <summary>
    /// The document persistence to manage 
    /// document data
    /// </summary>
    public class DocumentPersistence : IDocumentPersistence
    {
        /// <summary>
        /// Sets an existing document for a given task on inactive
        /// </summary>
        /// <param name="createTaskViewModel"></param>
        /// <param name="loginToken"></param>
        public void Create(CreateDocumentViewModel createDocumentViewModel, string loginToken)
        {
            Guard.ModelStateCheck(createDocumentViewModel);
            Guard.NotNullOrEmpty(loginToken);

            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "User not found");
                }

                var task = db.Tasks.Include(t => t.Documents)
                                   .Include(t => t.Subject)
                                   .ThenInclude(s => s.TeachingClass)
                                   .ThenInclude(t => t.AssignedAccounts)
                                   .SingleOrDefault(t => t.Id == createDocumentViewModel.TaskId);


                if (task == null)
                {
                    throw new PropertyExceptionCollection(nameof(task),
                        "Subject not found");
                }

                if (!task.Subject.TeachingClass.AssignedAccounts.Contains(account))
                {
                    throw new PropertyExceptionCollection(nameof(task),
                        "No permissions granted");
                }

                var document = new Document()
                {
                    File = createDocumentViewModel.File,
                    ContentType = createDocumentViewModel.ContentType,
                    DocumentType = createDocumentViewModel.DocumentType,
                    FileName = createDocumentViewModel.FileName,
                    Uploaded = DateTime.Now,
                    Uploader = account,
                    Task = task,
                    Active = true
                };

                task.Documents.Add(document);
                db.Documents.Add(document);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Sets an existing document for a given task on inactive
        /// </summary>
        /// <param name="createTaskViewModel"></param>
        /// <param name="loginToken"></param>
        public void Delete(int id, string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Account not found");
                }

                var document = db.Documents.Include(d => d.Task)
                                           .ThenInclude(t => t.Subject)
                                           .ThenInclude(t => t.TeachingClass)
                                           .SingleOrDefault(a => a.Id == id);

                if (document == null)
                {
                    throw new PropertyExceptionCollection(nameof(document), "The given document couldn't be found");
                }

                if (document.Task.Subject.TeachingClass.Admin != account)
                {
                    throw new PropertyExceptionCollection(nameof(account), "You are not permitted to this changes");
                }

                document.Active = false;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Get Document Overview for a given task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="loginToken"></param>
        public OverviewDocumentViewModel GetOverview(Int64 taskId, string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (SuperSmartDb db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Account not found");
                }

                var documents = db.Documents.Include(t => t.Task)
                                            .ThenInclude(t => t.Subject)
                                            .ThenInclude(t => t.TeachingClass)
                                            .ThenInclude(t => t.AssignedAccounts)
                                            .Where(d => d.Task.Subject.TeachingClass.AssignedAccounts
                                                    .Any(a => a.LoginToken == loginToken) &&
                                                         d.Task.Id == taskId &&
                                                         d.Active);

                var convertedDocuments = GetDocumentOverviewMapper(account).Map<List<DocumentViewModel>>(documents);

                OverviewDocumentViewModel overviewDocumentViewModel = new OverviewDocumentViewModel()
                {
                    TaskId = taskId,
                    Documents = convertedDocuments
                };

                return overviewDocumentViewModel;
            }
        }

        /// <summary>
        /// Get document to download
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="loginToken"></param>
        public DownloadDocumentViewModel Download(Int64 documentId, string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (SuperSmartDb db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Account not found");
                }

                var document = db.Documents.Include(t => t.Task)
                                           .ThenInclude(t => t.Subject)
                                           .ThenInclude(s => s.TeachingClass)
                                           .ThenInclude(t => t.AssignedAccounts)
                                            .SingleOrDefault(d => d.Task.Subject.TeachingClass.AssignedAccounts
                                                .Any(a => a.LoginToken == loginToken) && d.Id == documentId);

                if (document == null)
                {
                    throw new PropertyExceptionCollection(nameof(document), "Document not found");
                }

                if (!document.Task.Subject.TeachingClass.AssignedAccounts.Any(a => a.LoginToken == loginToken))
                {
                    throw new PropertyExceptionCollection(nameof(document), "User has no permissions to download document");
                }

                var downloadDocumentViewModel = this.GetDocumentDownloadMapper().Map<DownloadDocumentViewModel>(document);

                return downloadDocumentViewModel;
            }
        }


        /// <summary>
        /// Get document to manage
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="loginToken"></param>
        public ManageDocumentViewModel GetManagedDocument(Int64 documentId, string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (SuperSmartDb db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Account not found");
                }

                var document = db.Documents.Include(t => t.Task)
                                           .ThenInclude(t => t.Subject)
                                           .ThenInclude(s => s.TeachingClass)
                                           .ThenInclude(t => t.AssignedAccounts)
                                            .SingleOrDefault(d => d.Task.Subject.TeachingClass.AssignedAccounts
                                                .Any(a => a.LoginToken == loginToken) && d.Id == documentId);

                if (document == null)
                {
                    throw new PropertyExceptionCollection(nameof(document), "Document not found");
                }

                if (document.Uploader != account)
                {
                    throw new PropertyExceptionCollection(nameof(document), "User has no permissions to manage document");
                }

                var manageDocumentViewModel = this.GetDocumentManageMapper().Map<ManageDocumentViewModel>(document);

                return manageDocumentViewModel;
            }
        }

        /// <summary>
        /// Manage document
        /// </summary>
        /// <param name="manageDocumentViewModel"></param>
        /// <param name="loginToken"></param>
        public void Manage(ManageDocumentViewModel manageDocumentViewModel, string loginToken)
        {
            Guard.ModelStateCheck(manageDocumentViewModel);
            Guard.NotNullOrEmpty(loginToken);

            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "User not found");
                }

                var document = db.Documents.SingleOrDefault(itm => itm.Id == manageDocumentViewModel.Id);

                if (document == null)
                {
                    throw new PropertyExceptionCollection(nameof(document), "Document not found");
                }

                if (document.Uploader != account)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "No permissions granted");
                }

                if (manageDocumentViewModel.File != null)
                {
                    document.File = manageDocumentViewModel.File;
                    document.ContentType = manageDocumentViewModel.ContentType;
                    document.FileName = manageDocumentViewModel.FileName;
                }

                document.DocumentType = manageDocumentViewModel.DocumentType;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the document overview mapper to map documents to 
        /// a overview view modle
        /// </summary>
        /// <returns></returns>
        private IMapper GetDocumentOverviewMapper(Account account)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Document, DocumentViewModel>()
               .ForMember(vm => vm.Uploader, map => map.MapFrom(m => m.Uploader.FirstName + " " + m.Uploader.LastName))
                .ForMember(vm => vm.IsOwner, map => map.MapFrom(m => m.Uploader == account));
            }).CreateMapper();

            return mapper;
        }

        /// <summary>
        /// Gets the document manage mapper to map documents to 
        /// a manage view modle
        /// </summary>
        /// <returns></returns>
        private IMapper GetDocumentManageMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Document, ManageDocumentViewModel>();
            }).CreateMapper();

            return mapper;
        }

        /// <summary>
        /// Gets the document download mapper to map documents to 
        /// a download view modle
        /// </summary>
        /// <returns></returns>
        private IMapper GetDocumentDownloadMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Document, DownloadDocumentViewModel>();

            }).CreateMapper();

            return mapper;
        }
    }
}