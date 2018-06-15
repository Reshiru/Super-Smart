using SuperSmart.Core.Data.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSmart.Core.Data.Implementation
{
    public class AccountTask
    {
        /// <summary>
        /// The database generated identifier
        /// </summary>
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
