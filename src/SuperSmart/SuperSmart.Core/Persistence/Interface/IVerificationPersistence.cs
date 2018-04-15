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
        /// Generates new login token for user if model is valid
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns>The login token</returns>
        Guid Login(LoginViewModel loginViewModel);
        /// <summary>
        /// Registers a new user with the given data if model is valid
        /// </summary>
        /// <param name="registerViewModel"></param>
        void Register(RegisterViewModel registerViewModel);
    }
}
