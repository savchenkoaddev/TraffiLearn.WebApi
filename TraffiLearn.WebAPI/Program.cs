using Microsoft.EntityFrameworkCore;
using TraffiLearn.Application.ServiceContracts;
using TraffiLearn.Application.Services;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;
using TraffiLearn.Infrastructure.Options;
using TraffiLearn.Infrastructure.Repositories;

namespace TraffiLearn.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            var sqlServerSettings = builder.Configuration.GetSection(SqlServerSettings.CONFIG_KEY).Get<SqlServerSettings>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(sqlServerSettings.ConnectionString);
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
