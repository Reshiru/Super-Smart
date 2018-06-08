using SuperSmart.Core.Data.ViewModels;
using System.Collections.Generic;

namespace SuperSmart.Core.Persistence.Interface
{
    public interface IDashboardPersistence
    {
      
        /// <summary>
        /// Get DashboardData for given User
        /// </summary>
        /// <param name="userId"></param>
        DashboardViewModel GetDashboardData(int userId);
    }
}
