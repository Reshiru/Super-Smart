using System;
using SuperSmart.Core.Persistence.Interface;

namespace SuperSmart.Core.Persistence.Implementation
{
    public class VerificationPersistence : IVerificationPersistence
    {
        public bool Check(Guid sessionId)
        {
            throw new NotImplementedException();
        }

        public Guid Login(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
