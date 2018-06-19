using SuperSmart.Core.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Persistence.Interface
{
    public interface IUserPersistence
    {
        /// <summary>
        /// Get user by loginToken
        /// </summary>
        /// <param name="loginToken"></param>
        UserViewModel GetUserByLoginToken(string loginToken);

        /// <summary>
        /// Get full name by loginToken
        /// </summary>
        /// <param name="loginToken"></param>
        UserNameViewModel GetFullNameFromUser(string loginToken);
    }
}
