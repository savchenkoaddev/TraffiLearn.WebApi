using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Infrastructure.Database
{
    public sealed class ApplicationDbContext : DbContext
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

            modelBuilder.Entity<Question>().ComplexProperty(b => b.NumberDetails);
            modelBuilder.Entity<Question>().Navigation(q => q.DrivingCategories).AutoInclude();

            //Seed data
        }
    }
}
