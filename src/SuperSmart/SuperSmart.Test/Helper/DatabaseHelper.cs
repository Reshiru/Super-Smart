using SuperSmart.Core.Data.Connection;

namespace SuperSmart.Test.Helper
{
    public static class DatabaseHelper
    {
        /// <summary>
        /// Deletes the test database (only in test mode)
        /// </summary>
        public static void SecureDeleteDatabase()
        {
#if TEST
            using (var db = new SuperSmartDb())
            {
                db.Database.EnsureDeleted();
            }
#endif
        }
    }
}
