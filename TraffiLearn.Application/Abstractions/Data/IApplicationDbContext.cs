using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Abstractions.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<Topic> Topics { get; set; }

        public DbSet<Question> Questions { get; set; }
    }
}
