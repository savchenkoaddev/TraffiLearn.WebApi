using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Infrastructure.Persistence.Options;

namespace TraffiLearn.Infrastructure.Persistence
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IUnitOfWork
    {
        private readonly DbSettings _dbSettings;

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<DbSettings> dbSettings)
            : base(options)
        {
            _dbSettings = dbSettings.Value;
        }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_dbSettings.ConnectionString);
        }
    }
}
