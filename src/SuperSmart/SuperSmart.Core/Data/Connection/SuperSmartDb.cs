using Microsoft.EntityFrameworkCore;
using SuperSmart.Core.Data.Implementation;

namespace SuperSmart.Core.Data.Connection
{
    public class SuperSmartDb : DbContext
    {
        /****************************/
        /*Constructor*/
        /****************************/
        #region ctor
        public SuperSmartDb()
        {
            this.Database.EnsureCreated();
        }
        #endregion ctor
        public DbSet<Account> Accounts { get; set; }
        public DbSet<TeachingClass> TeachingClasses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Implementation.Task> Tasks { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Document> Documents { get; set; }
        /// <summary>
        /// The configuration for the database
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={ConnectionString}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasMany(a => a.AssignedClasses);
            modelBuilder.Entity<Account>().HasMany(a => a.RequestedClasses);
            modelBuilder.Entity<TeachingClass>().HasMany(a => a.AssignedAccounts);
            modelBuilder.Entity<TeachingClass>().HasMany(a => a.OpenRequests);
            modelBuilder.Entity<TeachingClass>().HasOne(a => a.Admin);
        }
        /// <summary>
        /// Returns the connection
        /// </summary>
        /// <returns></returns>
        public static string ConnectionString
        {
            get
            {
#if TEST
                return ".\\SuperSmartTest.db";
#else
                return ".\\SuperSmartRelease.db";
#endif
            }
        }
    }
}
