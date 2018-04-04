using SQLite.CodeFirst;
using SuperSmart.Core.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.Connection
{
    public class SuperSmartDb : DbContext
    {
        /****************************/
        /*Constructor*/
        /****************************/
        #region ctor
        /// <summary>
        /// The database constructor (initialize database location)
        /// </summary>
        public SuperSmartDb()
#if !Release
            : base("Local")
#else
            : base ("Release")
#endif
        { }
        #endregion ctor
        public DbSet<Account> Accounts { get; set; }
        public DbSet<TeachingClass> TeachingClasses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Implementation.Task> Tasks { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Document> Documents { get; set; }
        /// <summary>
        /// Overrides the model creation method and initializes the code first database
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Code first implementation
            Database.SetInitializer(new SqliteCreateDatabaseIfNotExists<SuperSmartDb>(modelBuilder));
        }
    }
}
