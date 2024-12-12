using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TraffiLearn.Application.UseCases.Users.Identity;
using TraffiLearn.Domain.Comments;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Regions;
using TraffiLearn.Domain.Routes;
using TraffiLearn.Domain.ServiceCenters;
using TraffiLearn.Domain.Shared.CanceledSubscriptions;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.Domain.Topics;
using TraffiLearn.Domain.Users;
using TraffiLearn.Infrastructure.Persistence.Outbox;
using TraffiLearn.SharedKernel.Primitives;
using Directory = TraffiLearn.Domain.Directories.Directory;

namespace TraffiLearn.Infrastructure.Persistence
{
    public sealed class ApplicationDbContext :
        IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<OutboxMessage> OutboxMessages { get; init; }

        public DbSet<Question> Questions { get; init; }

        public DbSet<Topic> Topics { get; init; }

        public DbSet<Ticket> Tickets { get; init; }

        public DbSet<Comment> Comments { get; init; }

        public DbSet<User> Users { get; init; }

        public DbSet<Region> Regions { get; init; }

        public DbSet<ServiceCenter> ServiceCenters { get; init; }

        public DbSet<Route> Routes { get; init; }

        public DbSet<Directory> Directories { get; init; }

        public DbSet<SubscriptionPlan> SubscriptionPlans { get; init; }

        public DbSet<CanceledSubscription> CanceledSubscriptions { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<DomainEvent>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
