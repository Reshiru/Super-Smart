using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Test
{
    [TestClass]
    public class DocumentTest
    {
        IDocumentPersistence documentPersistence = new DocumentPersistence();

        [TestMethod]
        public void CreateDocumentSucceed()
        {
            DatabaseHelper.SecureDeleteDatabase();

            try
            {
                var token = DatabaseHelper.GenerateFakeAccount();
                var teachingClassId = DatabaseHelper.GenerateFakeTeachingClass(token);
                var subjectId = DatabaseHelper.GenerateFakeSubject(teachingClassId);
                var taskId = DatabaseHelper.GenerateFakeTask(subjectId, token);

                documentPersistence.Create(new CreateDocumentViewModel()
                {
                    File = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 },
                    DocumentType = Core.Data.Enumeration.DocumentType.Document,
                    FileName = "Testfile",
                    TaskId = 1
                }, token);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }
    }
}
