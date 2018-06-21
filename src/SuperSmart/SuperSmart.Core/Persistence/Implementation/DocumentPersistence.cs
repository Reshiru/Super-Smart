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

                var convertedDocuments = GetDocumentOverviewMapper().Map<List<DocumentViewModel>>(documents);

                convertedDocuments.ForEach(itm =>
                {
                    itm.IsOwner = documents.Single(d => d.Id == itm.Id).Uploader == account;
                });

                OverviewDocumentViewModel overviewDocumentViewModel = new OverviewDocumentViewModel()
                {
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

                if (document.Task.Subject.TeachingClass.AssignedAccounts.Any(a => a.LoginToken == loginToken))
                {
                    throw new PropertyExceptionCollection(nameof(document), "User has no permissions to download document");
                }

                var downloadDocumentViewModel = GetDocumentOverviewMapper().Map<DownloadDocumentViewModel>(document);

                return downloadDocumentViewModel;
            }
        }

        /// <summary>
        /// Gets the document overview mapper to map documents to 
        /// a overview view modle
        /// </summary>
        /// <returns></returns>
        public IMapper GetDocumentOverviewMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Document, DocumentViewModel>()
               .ForMember(vm => vm.Filename, map => map.MapFrom(m => m.FileName))
               .ForMember(vm => vm.Uploader, map => map.MapFrom(m => m.Uploader.FirstName + " " + m.Uploader.LastName));

                cfg.CreateMap<Document, DocumentViewModel>()
                .ForMember(vm => vm.File, map => map.MapFrom(m => m.File))
                .ForMember(vm => vm.ContentType, map => map.MapFrom(m => m.ContentType))
                .ForMember(vm => vm.Filename, map => map.MapFrom(m => m.FileName));
            }).CreateMapper();

            return mapper;
        }
    }
}