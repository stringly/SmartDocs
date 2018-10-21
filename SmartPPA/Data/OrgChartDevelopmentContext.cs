using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SmartPPA.Data
{
    public partial class OrgChartDevelopmentContext : DbContext
    {
        public OrgChartDevelopmentContext()
        {
        }

        public OrgChartDevelopmentContext(DbContextOptions<OrgChartDevelopmentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Components> Components { get; set; }
        public virtual DbSet<MemberRanks> MemberRanks { get; set; }
        public virtual DbSet<Members> Members { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=OrgChartDevelopment;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Components>(entity =>
            {
                entity.HasKey(e => e.ComponentId);

                entity.HasIndex(e => e.ParentComponentComponentId);

                entity.HasOne(d => d.ParentComponentComponent)
                    .WithMany(p => p.InverseParentComponentComponent)
                    .HasForeignKey(d => d.ParentComponentComponentId);
            });

            modelBuilder.Entity<MemberRanks>(entity =>
            {
                entity.HasKey(e => e.RankId);
            });

            modelBuilder.Entity<Members>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.HasIndex(e => e.PositionId)
                    .HasName("IX_Members_PositionPostionId");

                entity.HasIndex(e => e.RankId);

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK_Members_Positions_PositionPostionId");

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.RankId);
            });

            modelBuilder.Entity<Positions>(entity =>
            {
                entity.HasKey(e => e.PositionId);

                entity.HasIndex(e => e.ParentComponentComponentId);

                entity.HasOne(d => d.ParentComponentComponent)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.ParentComponentComponentId);
            });
        }
    }
}
