using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models
{
    public class SmartDocContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartPPA.Models.SmartDocContext"/> class.
        /// </summary>
        /// <param name="options">A <see cref="T:Microsoft.EntityFrameWorkCore.DbContextOptions"/> of <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/></param>
        public SmartDocContext(DbContextOptions<SmartDocContext> options) : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartPPA.Models.SmartDocContext"/> class.
        /// </summary>
        /// <remarks>
        /// Parameterless Constructor
        /// </remarks>
        public SmartDocContext()
        {
        }
        public virtual DbSet<SmartUser> Users { get; set; }
        public virtual DbSet<SmartTemplate> Templates { get; set; }
        public virtual DbSet<SmartJob> Jobs { get; set; }
        public virtual DbSet<SmartPPA> PPAs { get; set; }
    }
}
