using Microsoft.EntityFrameworkCore;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SuperSmart.Core.Persistence.Implementation
{
    public class DocumentPersistence : IDocumentPersistence
    {
        public void Create(CreateDocumentViewModel createDocumentViewModel, string loginToken)
        {
            if (createDocumentViewModel == null)
            {
                throw new PropertyExceptionCollection(nameof(createDocumentViewModel), "Parameter cannot be null");
            }

            if (string.IsNullOrEmpty(loginToken))
            {
                throw new PropertyExceptionCollection(nameof(loginToken), "Parameter cannot be null");
            }

            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(createDocumentViewModel, new System.ComponentModel.DataAnnotations.ValidationContext(createDocumentViewModel, serviceProvider: null, items: null), validationResults, true))
            {
                throw new PropertyExceptionCollection(validationResults);
            }


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

        public void Delete(int id, string loginToken)
        {
            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Your account couldn't be found. Pleas try to relogin");
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
    }
}
