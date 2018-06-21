using SuperSmart.Core.Data.Enumeration;

namespace SuperSmart.Core.Data.Implementation
{
    /// <summary>
    /// Represents an account task entry,
    /// contains information about a task which is rated by 
    /// a user (ex. state from task changed)
    /// </summary>
    public class AccountTask
    {
        /// <summary>
        /// The database generated identifier
        /// </summary>
        /// ToDo: replace with int64
        public int Id { get; set; }

        /// <summary>
        /// The Task of a AccountTask
        /// </summary>
        public Task Task { get; set; }

        /// <summary>
        /// The Account of a AccountTask
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// The Status of a AccountTask
        /// </summary>
        public TaskStatus Status { get; set; }
    }
}
