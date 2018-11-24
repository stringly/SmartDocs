using Microsoft.EntityFrameworkCore;

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

        /// <summary>
        /// Gets or sets the Users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public virtual DbSet<SmartUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the Templates.
        /// </summary>
        /// <value>
        /// The templates.
        /// </value>
        public virtual DbSet<SmartTemplate> Templates { get; set; }

        /// <summary>
        /// Gets or sets the Jobs.
        /// </summary>
        /// <value>
        /// The jobs.
        /// </value>
        public virtual DbSet<SmartJob> Jobs { get; set; }

        /// <summary>
        /// Gets or sets the PPAs.
        /// </summary>
        /// <value>
        /// The pp as.
        /// </value>
        public virtual DbSet<SmartPPA> PPAs { get; set; }

        /// <summary>
        /// Gets or sets the Components.
        /// </summary>
        /// <value>
        /// The components.
        /// </value>
        public virtual DbSet<OrganizationComponent> Components { get; set; }
    }
}
