using KodyPromocyjneAPI.DataLayer;
using KodyPromocyjneAPI.DataLayer.Models;

namespace KodyPromocyjneAPI.BusinessLayer.Services
{
    public interface IChangeLogServices
    {
        Task AddChangeLogAsync(ChangeLog changeLog);
    }

    public class ChangeLogServices : IChangeLogServices
    {
        private readonly Func<IPromoCodesDbContext> _promoCodesDbContextFactoryMethod;

        public ChangeLogServices(Func<IPromoCodesDbContext> promoCodesDbContextFactoryMethod)
        {
            _promoCodesDbContextFactoryMethod = promoCodesDbContextFactoryMethod;
        }

        public async Task AddChangeLogAsync(ChangeLog changeLog)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                context.ChangeLogs.Add(changeLog);
                await context.SaveChangesAsync();
            }
        }
    }
}
