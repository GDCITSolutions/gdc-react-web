﻿// <auto-generated />
using System;
using BE.LocalAccountabilitySystem.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BE.LocalAccountabilitySystem.Schema.Migrations
{
    [DbContext(typeof(SampleContext))]
    [Migration("20240201194853_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BE.LocalAccountabilitySystem.Entities.Database.PasswordResetToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<int>("SystemStatusId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Token")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SystemStatusId");

                    b.HasIndex("UserId");

                    b.ToTable("PasswordResetToken", null, t =>
                        {
                            t.HasTrigger("PasswordResetTokenUpdateTrigger");
                        });
                });

            modelBuilder.Entity("BE.LocalAccountabilitySystem.Entities.Database.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime?>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<int>("SystemStatusId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("SystemStatusId");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "District Admin",
                            SystemStatusId = 1,
                            Value = "District Admin"
                        },
                        new
                        {
                            Id = 2,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "School Admin",
                            SystemStatusId = 1,
                            Value = "School Admin"
                        },
                        new
                        {
                            Id = 3,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Faculty",
                            SystemStatusId = 1,
                            Value = "Faculty"
                        },
                        new
                        {
                            Id = 4,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Parent",
                            SystemStatusId = 1,
                            Value = "Parent"
                        },
                        new
                        {
                            Id = 5,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Student",
                            SystemStatusId = 1,
                            Value = "Student"
                        });
                });

            modelBuilder.Entity("BE.LocalAccountabilitySystem.Entities.Database.SystemStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("SystemStatus", null, t =>
                        {
                            t.HasTrigger("SystemStatusUpdateTrigger");
                        });

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Active",
                            Value = "Active"
                        },
                        new
                        {
                            Id = 2,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Inactive",
                            Value = "Inactive"
                        },
                        new
                        {
                            Id = 3,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Removed",
                            Value = "Removed"
                        },
                        new
                        {
                            Id = 4,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Locked",
                            Value = "Locked"
                        });
                });

            modelBuilder.Entity("BE.LocalAccountabilitySystem.Entities.Database.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("LockedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Password")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Salt")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("SystemStatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.HasIndex("LockedByUserId");

                    b.HasIndex("SystemStatusId");

                    b.ToTable("User", null, t =>
                        {
                            t.HasTrigger("UserUpdateTrigger");
                        });
                });

            modelBuilder.Entity("BE.LocalAccountabilitySystem.Entities.Database.UserToRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime?>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("SystemStatusId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("SystemStatusId");

                    b.HasIndex("UserId");

                    b.ToTable("UserToRole", null, t =>
                        {
                            t.HasTrigger("UserToRoleUpdateTrigger");
                        });
                });

            modelBuilder.Entity("BE.LocalAccountabilitySystem.Entities.Database.PasswordResetToken", b =>
                {
                    b.HasOne("BE.LocalAccountabilitySystem.Entities.Database.SystemStatus", "SystemStatus")
                        .WithMany()
                        .HasForeignKey("SystemStatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BE.LocalAccountabilitySystem.Entities.Database.User", "User")
                        .WithMany("PasswordResetTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SystemStatus");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BE.LocalAccountabilitySystem.Entities.Database.Role", b =>
                {
                    b.HasOne("BE.LocalAccountabilitySystem.Entities.Database.SystemStatus", "SystemStatus")
                        .WithMany()
                        .HasForeignKey("SystemStatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SystemStatus");
                });

            modelBuilder.Entity("BE.LocalAccountabilitySystem.Entities.Database.User", b =>
                {
                    b.HasOne("BE.LocalAccountabilitySystem.Entities.Database.User", "LockedByUser")
                        .WithMany()
                        .HasForeignKey("LockedByUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BE.LocalAccountabilitySystem.Entities.Database.SystemStatus", "SystemStatus")
                        .WithMany()
                        .HasForeignKey("SystemStatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("LockedByUser");

                    b.Navigation("SystemStatus");
                });

            modelBuilder.Entity("BE.LocalAccountabilitySystem.Entities.Database.UserToRole", b =>
                {
                    b.HasOne("BE.LocalAccountabilitySystem.Entities.Database.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BE.LocalAccountabilitySystem.Entities.Database.SystemStatus", "SystemStatus")
                        .WithMany()
                        .HasForeignKey("SystemStatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BE.LocalAccountabilitySystem.Entities.Database.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("SystemStatus");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BE.LocalAccountabilitySystem.Entities.Database.User", b =>
                {
                    b.Navigation("PasswordResetTokens");

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
