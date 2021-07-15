﻿using de_exceptional_closures_core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace de_exceptional_closures_Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ReasonType> ReasonType { get; set; }
        public DbSet<ClosureReason> ClosureReason { get; set; }
        public DbSet<ApprovalType> ApprovalType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ReasonType>()
             .HasData(new ReasonType { Id = 1, Description = "Adverse weather" },
                        new ReasonType { Id = 2, Description = "Use as a polling station" },
                        new ReasonType { Id = 3, Description = "Utilities failure (e.g. water, electricity)" },
                        new ReasonType { Id = 4, Description = "Death of a member of staff, pupil or another person working at the school" },
                        new ReasonType { Id = 5, Description = "Other (inc. COVID-19; please enter start and proposed end date)" }
                        );

            modelBuilder.Entity<ApprovalType>()
                      .HasData(new ApprovalType { Id = 1, Description = "Pre-approved" },
                               new ApprovalType { Id = 2, Description = "Approval required" }
             );

            base.OnModelCreating(modelBuilder);
        }
    }
}
