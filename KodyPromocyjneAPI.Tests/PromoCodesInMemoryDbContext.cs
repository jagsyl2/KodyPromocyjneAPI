using KodyPromocyjneAPI.DataLayer;
using KodyPromocyjneAPI.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace KodyPromocyjneAPI.Tests
{
    public class PromoCodesInMemoryDbContext : DbContext, IPromoCodesDbContext
    {
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<ChangeLog> ChangeLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("KodyPromocyjne");
            optionsBuilder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        }
    }
}
