using EF_CodeFirst_Kursverwaltung.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_CodeFirst_Kursverwaltung
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<KursContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MeineDB")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Kurs}/{action=Index}/{id?}");

            app.Run();
        }
    }
}