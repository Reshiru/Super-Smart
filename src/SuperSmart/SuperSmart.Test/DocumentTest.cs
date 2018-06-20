using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Data.Enumeration;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Builder;
using System;

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
                    TaskId = task.Id
                }, account.LoginToken);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }
    }
}
