using TraffiLearn.Application;
using TraffiLearn.Infrastructure;
using TraffiLearn.WebApp.Options;

namespace TraffiLearn.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.Configure<PaginationSettings>(builder.Configuration.GetSection(PaginationSettings.CONFIG_KEY));

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllers();

            app.Run();
        }
    }
}
