using KodyPromocyjneAPI.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace KodyPromocyjneAPI.DataLayer
{
    public interface IPromoCodesDbContext : IDisposable
    {
        DbSet<PromoCode> PromoCodes { get; set;}
        DbSet<ChangeLog> ChangeLogs { get; set; }
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }

    public class PromoCodesDbContext : DbContext, IPromoCodesDbContext
    {
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<ChangeLog> ChangeLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=KodyPromocyjneAPI;Trusted_Connection=True;");
        }
    }
}
