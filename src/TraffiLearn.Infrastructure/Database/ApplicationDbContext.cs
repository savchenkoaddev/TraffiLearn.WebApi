using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Infrastructure.Options;

namespace TraffiLearn.Infrastructure.Database
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IUnitOfWork
    {
        private readonly SqlServerSettings _sqlServerSettings;

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<SqlServerSettings> sqlServerSettings)
            : base(options)
        {
            _sqlServerSettings = sqlServerSettings.Value;
        }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_sqlServerSettings.ConnectionString);
        }
    }
}
