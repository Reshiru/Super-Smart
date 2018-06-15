using SuperSmart.Core.Data.ViewModels;
using System.Collections.Generic;

namespace SuperSmart.Core.Persistence.Interface
{
    public interface IDashboardPersistence
    {

        /// <summary>
        /// Get DashboardData for given User
        /// </summary>
        /// <param name="loginToken"></param>
        DashboardViewModel GetDashboardData(string loginToken);
    }
}
