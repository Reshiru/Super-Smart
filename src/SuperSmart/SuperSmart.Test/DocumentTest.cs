using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Data.Enumeration;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Builder;
using System;
using System.Linq;

namespace SuperSmart.Test
{
    [TestClass]
    public class DocumentTest
    {
        IDocumentPersistence documentPersistence = new DocumentPersistence();

        [TestMethod]
        public void CreateDocumentSucceed()
        {
            var filename = "Testfile";
            var file = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            var documenType = DocumentType.Document;
            var contentType = "image/jpg";

            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            var subject = new SubjectBuilder().WithTeachingClass(teachingClass)
                                              .Build();

            var task = new TaskBuilder().WithOwner(account)
                                        .WithSubject(subject)
                                        .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .WithSubject(subject)
                                 .WithTask(task)
                                 .Build();

            try
            {
                documentPersistence.Create(new CreateDocumentViewModel()
                {
                    File = file,
                    DocumentType = documenType,
                    FileName = filename,
                    TaskId = task.Id,
                    ContentType = contentType
                }, account.LoginToken);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }


        [TestMethod]
        public void CreateDocumentWithNullParameterPropertyException()
        {
            CreateDocumentViewModel createDocumentViewModel = null;
            string loginToken = null;

            try
            {
                documentPersistence.Create(createDocumentViewModel, loginToken);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void CreateDocumentValidAccountValidTaskEmptyFileThrowPropertyException()
        {
            var filename = "Testfile";
            var file = new byte[] { };
            var documenType = DocumentType.Document;
            var contentType = "image/jpg";

            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            var subject = new SubjectBuilder().WithTeachingClass(teachingClass)
                                              .Build();

            var task = new TaskBuilder().WithOwner(account)
                                        .WithSubject(subject)
                                        .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .WithSubject(subject)
                                 .WithTask(task)
                                 .Build();

            try
            {
                documentPersistence.Create(new CreateDocumentViewModel()
                {
                    File = file,
                    DocumentType = documenType,
                    FileName = filename,
                    TaskId = task.Id,
                    ContentType = contentType
                }, account.LoginToken);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void OverviewDocumentSucceed()
        {

            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            var subject = new SubjectBuilder().WithTeachingClass(teachingClass)
                                              .Build();

            var task = new TaskBuilder().WithOwner(account)
                                        .WithSubject(subject)
                                        .Build();

            var document = new DocumentBuilder().WithOwner(account)
                                                .WithTask(task)
                                                .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .WithSubject(subject)
                                 .WithTask(task)
                                 .WithDocument(document)
                                 .Build();

            try
            {
                OverviewDocumentViewModel vm = documentPersistence.GetOverview(task.Id, account.LoginToken);

                if (vm.Documents.Any(d => d.IsOwner))
                    Assert.IsTrue(true);
                else
                    Assert.IsTrue(false, "User is not owner of document");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }
    }
}
