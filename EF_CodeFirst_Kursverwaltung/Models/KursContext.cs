using Microsoft.EntityFrameworkCore;

namespace EF_CodeFirst_Kursverwaltung.Models
{
    public class KursContext : DbContext
    {
        public DbSet<Kurs> Kurse { get; set; }

        public KursContext(DbContextOptions<KursContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}