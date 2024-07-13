using Microsoft.EntityFrameworkCore;
using TraffiLearn.Application.Data;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Infrastructure.Database
{
    public sealed class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Question>().ComplexProperty(q => q.TitleDetails);
        }
    }
}
