﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using portofolio_aspnet_core.Data;

#nullable disable

namespace portofolio_aspnet_core.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211126095458_CreateInitialTable")]
    partial class CreateInitialTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("portofolio_aspnet_core.Models.Category", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id_category");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("category");
                });

            modelBuilder.Entity("portofolio_aspnet_core.Models.Member", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id_member");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("text")
                        .HasColumnName("profile_picture");

                    b.HasKey("Id");

                    b.ToTable("member");
                });

            modelBuilder.Entity("portofolio_aspnet_core.Models.Project", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id_project");

                    b.Property<string>("Content")
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Url")
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.ToTable("project");
                });

            modelBuilder.Entity("portofolio_aspnet_core.Models.ProjectCategory", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id_project_category");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("id_category");

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("id_project");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProjectId");

                    b.ToTable("project_category");
                });

            modelBuilder.Entity("portofolio_aspnet_core.Models.ProjectImage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id_project_image");

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("id_project");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("project_image");
                });

            modelBuilder.Entity("portofolio_aspnet_core.Models.ProjectMember", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id_project_member");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("id_member");

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("id_project");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("ProjectId");

                    b.ToTable("project_member");
                });

            modelBuilder.Entity("portofolio_aspnet_core.Models.ProjectCategory", b =>
                {
                    b.HasOne("portofolio_aspnet_core.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("portofolio_aspnet_core.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("portofolio_aspnet_core.Models.ProjectImage", b =>
                {
                    b.HasOne("portofolio_aspnet_core.Models.Project", "Project")
                        .WithMany("ProjectImages")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("portofolio_aspnet_core.Models.ProjectMember", b =>
                {
                    b.HasOne("portofolio_aspnet_core.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("portofolio_aspnet_core.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("portofolio_aspnet_core.Models.Project", b =>
                {
                    b.Navigation("ProjectImages");
                });
#pragma warning restore 612, 618
        }
    }
}
