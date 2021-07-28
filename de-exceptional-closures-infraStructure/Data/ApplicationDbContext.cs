using de_exceptional_closures_core.Entities;
using de_exceptional_closures_infraStructure.Data;
using dss_common.Extensions;
using dss_common.Functional;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace de_exceptional_closures_Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _services;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor, IServiceProvider services)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _services = services;
        }

        public DbSet<ReasonType> ReasonType { get; set; }
        public DbSet<ClosureReason> ClosureReason { get; set; }
        public DbSet<ApprovalType> ApprovalType { get; set; }
        public DbSet<RejectionReason> RejectionReason { get; set; }
        public DbSet<AutoApprovalList> AutoApprovalList { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<ReasonType>()
             .HasData(new ReasonType { Id = 1, Description = "Adverse weather" },
                        new ReasonType { Id = 2, Description = "Use as a polling station" },
                        new ReasonType { Id = 3, Description = "Utilities failure (e.g. water, electricity)" },
                        new ReasonType { Id = 4, Description = "Death of a member of staff, pupil or another person working at the school" },
                        new ReasonType { Id = 5, Description = "Other (inc. COVID-19; please enter start and proposed end date)" }
                        );

            builder.Entity<ApprovalType>()
                      .HasData(new ApprovalType { Id = 1, Description = "Pre-approved" },
                               new ApprovalType { Id = 2, Description = "Approval required" }
             );

            builder.Entity<RejectionReason>()
               .HasData(new RejectionReason { Id = 1, Description = "School development" },
                        new RejectionReason { Id = 2, Description = "Half day" },
                        new RejectionReason { Id = 3, Description = "Split site - other site operational" },
                        new RejectionReason { Id = 4, Description = "Building work" },
                        new RejectionReason { Id = 5, Description = "Move premises" },
                        new RejectionReason { Id = 6, Description = "Some classes still in" },
                        new RejectionReason { Id = 7, Description = "Planning day" },
                        new RejectionReason { Id = 8, Description = "School open for a couple of hours" },
                        new RejectionReason { Id = 9, Description = "Staff in school" },
                        new RejectionReason { Id = 10, Description = "Pupils in to sit exams" },
                        new RejectionReason { Id = 11, Description = "Strike day" },
                        new RejectionReason { Id = 12, Description = "Bank or Public Holiday" },
                        new RejectionReason { Id = 13, Description = "Wrong date" },
                        new RejectionReason { Id = 14, Description = "Not required" },
                        new RejectionReason { Id = 15, Description = "Does not meet criteria" }
             );

            builder.Entity<ClosureReason>().HasQueryFilter(f => EF.Property<string>(f, "UserId") == GetCurrentUserId().GetAwaiter().GetResult().Value.Id);

            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => (e.Entity is BaseUserEntity<int>) && (
                                e.State == EntityState.Added
                                || e.State == EntityState.Modified
                                || e.State == EntityState.Deleted));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added && entityEntry.Metadata.GetProperties().Any(p => p.Name == "UserId"))
                {
                    var userResult = await GetCurrentUserId();
                    if (userResult.IsSuccess)
                    {
                        entityEntry.CurrentValues["UserId"] = userResult.Value.Id;
                        entityEntry.CurrentValues["UserEmail"] = userResult.Value.Email;

                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<Result<UserData>> GetCurrentUserId()
        {
            var manager = _services.GetRequiredService<UserManager<IdentityUser>>();

            var userResult = (await manager.GetUserAsync(_httpContextAccessor.HttpContext.User)).ToMaybe();
            UserData userData = new UserData();
            userData.Id = userResult.Value.Id;
            userData.Email = userResult.Value.Email;

            return !userResult.HasValue ? Result.Fail<UserData>("User not found") : Result.Ok(userData);
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}