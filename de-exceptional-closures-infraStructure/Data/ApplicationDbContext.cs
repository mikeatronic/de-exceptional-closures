using de_exceptional_closures_core.Entities;
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
        private IServiceProvider _services;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor, IServiceProvider services)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _services = services;
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

            modelBuilder.Entity<ClosureReason>().HasQueryFilter(f => EF.Property<string>(f, "UserId") == GetCurrentUserId().GetAwaiter().GetResult().Value);

            base.OnModelCreating(modelBuilder);
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
                if (entityEntry.State == EntityState.Added)
                    if (entityEntry.Metadata.GetProperties().Any(p => p.Name == "UserId"))
                    {
                        var userResult = await GetCurrentUserId();
                        if (userResult.IsSuccess)
                            entityEntry.CurrentValues["UserId"] = userResult.Value;
                    }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<Result<string>> GetCurrentUserId()
        {
            var manager = _services.GetRequiredService<UserManager<IdentityUser>>();

            var userResult = (await manager.GetUserAsync(_httpContextAccessor.HttpContext.User)).ToMaybe();

            return !userResult.HasValue ? Result.Fail<string>("User not found") : Result.Ok(userResult.Value.Id);
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}