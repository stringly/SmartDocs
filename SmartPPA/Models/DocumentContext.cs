using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models
{
    public class DocumentContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartPPA.Models.DocumentContext"/> class.
        /// </summary>
        /// <param name="options">A <see cref="T:Microsoft.EntityFrameWorkCore.DbContextOptions"/> of <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/></param>
        public DocumentContext(DbContextOptions<DocumentContext> options) : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartPPA.Models.DocumentContext"/> class.
        /// </summary>
        /// <remarks>
        /// Parameterless Constructor
        /// </remarks>
        public DocumentContext()
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<SmartDocument> Documents { get; set; }
        public virtual DbSet<SmartTemplate> Templates { get; set; }
    }
}
