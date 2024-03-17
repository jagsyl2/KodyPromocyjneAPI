using KodyPromocyjneAPI.DataLayer;
using KodyPromocyjneAPI.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace KodyPromocyjneAPI.BusinessLayer.Services
{
    public interface IChangeLogServices
    {
        Task AddChangeLogAsync(ChangeLog changeLog);
        Task AddChangeLogAsync(string tableName, string description, int codeId, string newValue);
        Task AddChangeLogAsync(IPromoCodesDbContext context, string tableName, string description, PromoCode code);
        Task<List<ChangeLog>> GetChangeLogsByCodeIdAsync(int codeId);
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

        public async Task AddChangeLogAsync(string tableName, string description, int codeId, string newValue)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                context.ChangeLogs.Add(new ChangeLog
                {
                    TableName = tableName,
                    Description = description,
                    PromoCodeId = codeId,
                    NewValue = newValue,
                    DateChanged = DateTime.Now,
                });
                await context.SaveChangesAsync();
            }
        }

        public async Task AddChangeLogAsync(IPromoCodesDbContext context, string tableName, string description, PromoCode code)
        {
            context.ChangeLogs.Add(new ChangeLog
            {
                TableName = tableName,
                Description = description,
                PromoCodeId = code.Id,
                NewValue = code.NumberOfPossibleUses.ToString(),
                DateChanged = DateTime.Now,
            });
            await context.SaveChangesAsync();
        }

        public async Task<List<ChangeLog>> GetChangeLogsByCodeIdAsync(int codeId)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                return await context.ChangeLogs
                    .Where(cl => cl.PromoCodeId == codeId)
                    .ToListAsync();
            }
        }
    }
}
