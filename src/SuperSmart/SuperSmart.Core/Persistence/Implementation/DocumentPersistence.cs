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

                var document = db.Documents.SingleOrDefault(a => a.Id == id);

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

                var documentQuery = db.Documents.Where(d => d.Task.Subject.TeachingClass.AssignedAccounts.Any(a => a.LoginToken == loginToken) && d.Task.Id == taskId);

                List<DocumentViewModel> documents = this.GetMapper().Map<List<DocumentViewModel>>(documentQuery);

                documents.ForEach(itm =>
                {
                    itm.IsOwner = documentQuery.Single(d => d.Id == itm.Id).Uploader == account;
                });

                OverviewDocumentViewModel overviewDocumentViewModel = new OverviewDocumentViewModel()
                {
                    Documents = documents
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

                var document = db.Documents.SingleOrDefault(d => d.Id == documentId);

                if (document == null)
                {
                    throw new PropertyExceptionCollection(nameof(document), "Document not found");
                }

                if (document.Task.Subject.TeachingClass.AssignedAccounts.Any(a => a.LoginToken == loginToken))
                {
                    throw new PropertyExceptionCollection(nameof(document), "User has no permissions to download document");
                }

                return this.GetMapper().Map<DownloadDocumentViewModel>(document);
            }
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Document, DocumentViewModel>()
               .ForMember(vm => vm.File, map => map.MapFrom(m => m.File))
               .ForMember(vm => vm.ContentType, map => map.MapFrom(m => m.ContentType))
               .ForMember(vm => vm.Filename, map => map.MapFrom(m => m.FileName))
               .ForMember(vm => vm.DocumentType, map => map.MapFrom(m => m.DocumentType))
               .ForMember(vm => vm.Uploaded, map => map.MapFrom(m => m.Uploaded))
               .ForMember(vm => vm.Uploader, map => map.MapFrom(m => m.Uploader.FirstName + " " + m.Uploader.LastName));

                cfg.CreateMap<Document, DocumentViewModel>()
                .ForMember(vm => vm.File, map => map.MapFrom(m => m.File))
                .ForMember(vm => vm.ContentType, map => map.MapFrom(m => m.ContentType))
                .ForMember(vm => vm.Filename, map => map.MapFrom(m => m.FileName));
            });

            return config.CreateMapper();
        }
    }
}