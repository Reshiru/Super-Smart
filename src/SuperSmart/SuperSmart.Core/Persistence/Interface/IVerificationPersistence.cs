using SuperSmart.Core.Data.ViewModels;
using System;

namespace SuperSmart.Core.Persistence.Interface
{
    /// <summary>
    /// The verification peristence layer interface
    /// </summary>
    public interface IVerificationPersistence
    {
        /// <summary>
        /// CHecks if the username with the given password exists,
        /// if true returns new session id
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Guid Login(LoginViewModel loginViewModel);
        void Register(RegisterViewModel registerViewModel);
    }
}
