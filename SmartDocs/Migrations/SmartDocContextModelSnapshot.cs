﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartDocs.Models;

namespace SmartDocs.Migrations
{
    [DbContext(typeof(SmartDocContext))]
    partial class SmartDocContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartDocs.Models.OrganizationComponent", b =>
                {
                    b.Property<int>("ComponentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("DepartmentCode");

                    b.Property<string>("Name");

                    b.HasKey("ComponentId");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("SmartDocs.Models.SmartJob", b =>
                {
                    b.Property<int>("JobId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("JobData")
                        .HasColumnType("xml");

                    b.Property<string>("JobName");

                    b.HasKey("JobId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("SmartDocs.Models.SmartPPA", b =>
                {
                    b.Property<int>("PPAId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssessmentComments");

                    b.Property<int?>("CategoryScore_1");

                    b.Property<int?>("CategoryScore_2");

                    b.Property<int?>("CategoryScore_3");

                    b.Property<int?>("CategoryScore_4");

                    b.Property<int?>("CategoryScore_5");

                    b.Property<int?>("CategoryScore_6");

                    b.Property<DateTime>("Created");

                    b.Property<string>("DepartmentDivision");

                    b.Property<string>("DepartmentDivisionCode");

                    b.Property<string>("DepartmentIdNumber");

                    b.Property<string>("DocumentName");

                    b.Property<string>("EmployeeFirstName");

                    b.Property<string>("EmployeeLastName");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("FormData")
                        .HasColumnType("xml");

                    b.Property<int>("JobId");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("OwnerUserId");

                    b.Property<string>("PayrollIdNumber");

                    b.Property<string>("PositionNumber");

                    b.Property<string>("RecommendationComments");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("SupervisedByEmployee");

                    b.Property<int>("TemplateId");

                    b.Property<string>("WorkplaceAddress");

                    b.HasKey("PPAId");

                    b.HasIndex("JobId");

                    b.HasIndex("OwnerUserId");

                    b.HasIndex("TemplateId");

                    b.ToTable("PPAs");
                });

            modelBuilder.Entity("SmartDocs.Models.SmartTemplate", b =>
                {
                    b.Property<int>("TemplateId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("DataStream");

                    b.Property<string>("DocumentName");

                    b.HasKey("TemplateId");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("SmartDocs.Models.SmartUser", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BlueDeckId");

                    b.Property<string>("DisplayName");

                    b.Property<string>("LogonName");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SmartDocs.Models.SmartPPA", b =>
                {
                    b.HasOne("SmartDocs.Models.SmartJob", "Job")
                        .WithMany()
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartDocs.Models.SmartUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartDocs.Models.SmartTemplate", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
