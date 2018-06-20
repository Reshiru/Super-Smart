using Microsoft.EntityFrameworkCore;
using SuperSmart.Core.Data.Implementation;

namespace SuperSmart.Core.Data.Connection
{
    /// <summary>
    /// The database context for the
    /// super smart database
    /// </summary>
    public class SuperSmartDb : DbContext
    {
        /// <summary>
        /// Initializes a new instance for 
        /// the super smart database context 
        /// </summary>
        public SuperSmartDb()
        {
            this.Database.EnsureCreated();
        }

        /// <summary>
        /// Represents the accounts database table
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Represents the teaching class database table
        /// </summary>
        public DbSet<TeachingClass> TeachingClasses { get; set; }

        /// <summary>
        /// Represents the subjects database table
        /// </summary>
        public DbSet<Subject> Subjects { get; set; }

        /// <summary>
        /// Represents the tasks database table
        /// </summary>
        public DbSet<Task> Tasks { get; set; }

        /// <summary>
        /// Represents the appointments database table
        /// </summary>
        public DbSet<Appointment> Appointments { get; set; }

        /// <summary>
        /// Represents the documents database table
        /// </summary>
        public DbSet<Document> Documents { get; set; }

        /// <summary>
        /// Represents the accountTasks database table
        /// ToDo: Rename table
        /// </summary>
        public DbSet<AccountTask> AccountTask { get; set; }

        /// <summary>
        /// The configuration for the database
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={ConnectionString}");
        }

        /// <summary>
        /// Setup the model creation keys
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasMany(a => a.AssignedClasses);
            modelBuilder.Entity<Account>().HasMany(a => a.RequestedClasses);
            modelBuilder.Entity<Account>().HasMany(a => a.AccountTasks);
            modelBuilder.Entity<TeachingClass>().HasMany(a => a.AssignedAccounts);
            modelBuilder.Entity<TeachingClass>().HasMany(a => a.OpenRequests);
            modelBuilder.Entity<TeachingClass>().HasOne(a => a.Admin);
            modelBuilder.Entity<Task>().HasMany(a => a.TaskAccounts);
            modelBuilder.Entity<AccountTask>().HasOne(a => a.Task);
            modelBuilder.Entity<AccountTask>().HasOne(a => a.Account);
        }

        /// <summary>
        /// Returns the connection string 
        /// (The database file path)
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