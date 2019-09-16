using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SmartDocs.OldModels
{
    public partial class SmartDocsContext : DbContext
    {
        public SmartDocsContext()
        {
        }

        public SmartDocsContext(DbContextOptions<SmartDocsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Components> Components { get; set; }
        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<Ppas> Ppas { get; set; }
        public virtual DbSet<Templates> Templates { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=PGPDSOD.co.pg.md.us;Database=SmartDocs;Trusted_Connection=True;MultipleActiveResultSets=true;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Components>(entity =>
            {
                entity.HasKey(e => e.ComponentId);
            });

            modelBuilder.Entity<Jobs>(entity =>
            {
                entity.HasKey(e => e.JobId);

                entity.Property(e => e.JobData).HasColumnType("xml");
            });

            modelBuilder.Entity<Ppas>(entity =>
            {
                entity.HasKey(e => e.Ppaid);

                entity.ToTable("PPAs");

                entity.HasIndex(e => e.JobId);

                entity.HasIndex(e => e.OwnerUserId);

                entity.HasIndex(e => e.TemplateId);

                entity.Property(e => e.Ppaid).HasColumnName("PPAId");

                entity.Property(e => e.CategoryScore1).HasColumnName("CategoryScore_1");

                entity.Property(e => e.CategoryScore2).HasColumnName("CategoryScore_2");

                entity.Property(e => e.CategoryScore3).HasColumnName("CategoryScore_3");

                entity.Property(e => e.CategoryScore4).HasColumnName("CategoryScore_4");

                entity.Property(e => e.CategoryScore5).HasColumnName("CategoryScore_5");

                entity.Property(e => e.CategoryScore6).HasColumnName("CategoryScore_6");

                entity.Property(e => e.Created).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.EndDate).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.Modified).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.StartDate).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.Ppas)
                    .HasForeignKey(d => d.JobId);

                entity.HasOne(d => d.OwnerUser)
                    .WithMany(p => p.Ppas)
                    .HasForeignKey(d => d.OwnerUserId);

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.Ppas)
                    .HasForeignKey(d => d.TemplateId);
            });

            modelBuilder.Entity<Templates>(entity =>
            {
                entity.HasKey(e => e.TemplateId);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);
            });
        }
    }
}
