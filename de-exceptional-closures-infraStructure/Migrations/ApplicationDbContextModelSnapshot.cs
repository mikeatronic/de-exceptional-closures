﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using de_exceptional_closures_Infrastructure.Data;

namespace de_exceptional_closures_Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(128) CHARACTER SET utf8mb4")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(128) CHARACTER SET utf8mb4")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(128) CHARACTER SET utf8mb4")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(128) CHARACTER SET utf8mb4")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("de_exceptional_closures_core.Entities.AdminApprovalList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("AdminApprovalList");
                });

            modelBuilder.Entity("de_exceptional_closures_core.Entities.ApprovalType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("ApprovalType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Pre-approved"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Approval required"
                        });
                });

            modelBuilder.Entity("de_exceptional_closures_core.Entities.AutoApprovalList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("AutoApprovalList");
                });

            modelBuilder.Entity("de_exceptional_closures_core.Entities.ClosureReason", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("ApprovalDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("ApprovalTypeId")
                        .HasColumnType("int");

                    b.Property<bool?>("Approved")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("CovidQ1")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CovidQ2")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CovidQ3")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CovidQ4")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CovidQ5")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateFrom")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateTo")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("InstitutionName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("OtherReason")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("OtherReasonCovid")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("ReasonTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("RejectionReasonId")
                        .HasColumnType("int");

                    b.Property<string>("Srn")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserEmail")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalTypeId");

                    b.HasIndex("ReasonTypeId");

                    b.HasIndex("RejectionReasonId");

                    b.ToTable("ClosureReason");
                });

            modelBuilder.Entity("de_exceptional_closures_core.Entities.ReasonType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool?>("ApprovalRequired")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("ReasonType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ApprovalRequired = false,
                            Description = "Adverse weather"
                        },
                        new
                        {
                            Id = 2,
                            ApprovalRequired = false,
                            Description = "Use as a polling station"
                        },
                        new
                        {
                            Id = 3,
                            ApprovalRequired = false,
                            Description = "Utilities failure (e.g. water, electricity)"
                        },
                        new
                        {
                            Id = 4,
                            ApprovalRequired = false,
                            Description = "Death of a member of staff, pupil or another person working at the school"
                        },
                        new
                        {
                            Id = 5,
                            ApprovalRequired = true,
                            Description = "Other"
                        },
                        new
                        {
                            Id = 6,
                            ApprovalRequired = true,
                            Description = "Covid"
                        });
                });

            modelBuilder.Entity("de_exceptional_closures_core.Entities.RejectionReason", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("RejectionReason");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "School development"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Half day"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Split site - other site operational"
                        },
                        new
                        {
                            Id = 4,
                            Description = "Building work"
                        },
                        new
                        {
                            Id = 5,
                            Description = "Move premises"
                        },
                        new
                        {
                            Id = 6,
                            Description = "Some classes still in"
                        },
                        new
                        {
                            Id = 7,
                            Description = "Planning day"
                        },
                        new
                        {
                            Id = 8,
                            Description = "School open for a couple of hours"
                        },
                        new
                        {
                            Id = 9,
                            Description = "Staff in school"
                        },
                        new
                        {
                            Id = 10,
                            Description = "Pupils in to sit exams"
                        },
                        new
                        {
                            Id = 11,
                            Description = "Strike day"
                        },
                        new
                        {
                            Id = 12,
                            Description = "Bank or Public Holiday"
                        },
                        new
                        {
                            Id = 13,
                            Description = "Wrong date"
                        },
                        new
                        {
                            Id = 14,
                            Description = "Not required"
                        },
                        new
                        {
                            Id = 15,
                            Description = "Does not meet criteria"
                        });
                });

            modelBuilder.Entity("de_exceptional_closures_infraStructure.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("InstitutionName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("InstitutionReference")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("de_exceptional_closures_infraStructure.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("de_exceptional_closures_infraStructure.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("de_exceptional_closures_infraStructure.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("de_exceptional_closures_infraStructure.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("de_exceptional_closures_core.Entities.ClosureReason", b =>
                {
                    b.HasOne("de_exceptional_closures_core.Entities.ApprovalType", "ApprovalType")
                        .WithMany()
                        .HasForeignKey("ApprovalTypeId");

                    b.HasOne("de_exceptional_closures_core.Entities.ReasonType", "ReasonType")
                        .WithMany()
                        .HasForeignKey("ReasonTypeId");

                    b.HasOne("de_exceptional_closures_core.Entities.RejectionReason", "RejectionReason")
                        .WithMany()
                        .HasForeignKey("RejectionReasonId");
                });
#pragma warning restore 612, 618
        }
    }
}
