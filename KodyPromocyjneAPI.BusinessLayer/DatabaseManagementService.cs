using KodyPromocyjneAPI.DataLayer;

namespace KodyPromocyjneAPI.BusinessLayer
{
    public interface IDatabaseManagementService
    {
        void EnsureDatabaseCreation();
    }

    public class DatabaseManagementService : IDatabaseManagementService
    {
        private readonly Func<IPromoCodesDbContext> _promoCodesDbContextFactoryMethod;

        public DatabaseManagementService(Func<IPromoCodesDbContext> promoCodesDbContextFactoryMethod)
        {
            _promoCodesDbContextFactoryMethod = promoCodesDbContextFactoryMethod;
        }

        public void EnsureDatabaseCreation()
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}