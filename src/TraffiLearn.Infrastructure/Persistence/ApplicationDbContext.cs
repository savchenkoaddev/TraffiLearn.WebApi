using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Regions;
using TraffiLearn.Domain.Aggregates.Routes;
using TraffiLearn.Domain.Aggregates.ServiceCenters;
using TraffiLearn.Domain.Aggregates.SubscriptionPlans;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Infrastructure.Persistence.Outbox;
using Directory = TraffiLearn.Domain.Aggregates.Directories.Directory;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<DomainEvent>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
