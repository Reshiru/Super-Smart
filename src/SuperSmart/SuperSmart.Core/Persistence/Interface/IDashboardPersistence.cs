using SuperSmart.Core.Data.ViewModels;

namespace SuperSmart.Core.Persistence.Interface
{
    /// <summary>
    /// The dashboard persistence to manage 
    /// the dashboard data
    /// </summary>
    public interface IDashboardPersistence
    {
        /// <summary>
        /// Get DashboardData for given User
        /// </summary>
        /// <param name="loginToken"></param>
        DashboardViewModel GetDashboardData(string loginToken);
    }
}