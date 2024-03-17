using KodyPromocyjneAPI.DataLayer;
using KodyPromocyjneAPI.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace KodyPromocyjneAPI.BusinessLayer.Services
{
    public interface  IPromoCodeServices
    {
        Task AddAsync(PromoCode code);
        Task<bool> CheckActiveCodeExistenceAsync(PromoCode code);
        Task<bool> CheckActiveCodeExistenceAsync(string name);
        Task<bool> CheckCodeByIdExistenceAsync(int id);
        Task DeleteAsync(PromoCode promoCode);
        Task<List<PromoCode>> GetAllCodesAsync();
        Task<PromoCode?> GetCodeByIdAsync(int id);
        Task<PromoCode> GetCodeByIdWithTransactionAsync(int id);
        Task SetPromoCodeToInactiveAsync(int id);
        Task UpdatePromoCodeNameAsync(int id, string newName);
    }

    public class PromoCodeServices : IPromoCodeServices
    {
        private readonly Func<IPromoCodesDbContext> _promoCodesDbContextFactoryMethod;
        private readonly IChangeLogServices _changeLogServices;

        public PromoCodeServices(
            Func<IPromoCodesDbContext> promoCodesDbContextFactoryMethod,
            IChangeLogServices changeLogServices)
        {
            _promoCodesDbContextFactoryMethod = promoCodesDbContextFactoryMethod;
            _changeLogServices = changeLogServices;
        }

        public async Task AddAsync(PromoCode code)
        {
            code.Code = Guid.NewGuid();

            using (var context = _promoCodesDbContextFactoryMethod())
            {
                context.PromoCodes.Add(code);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> CheckActiveCodeExistenceAsync(PromoCode code)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                return await context.PromoCodes
                    .AnyAsync(c => c.Name == code.Name && c.IsActive);
            }
        }

        public async Task<bool> CheckActiveCodeExistenceAsync(string name)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                return await context.PromoCodes
                    .AnyAsync(c => c.Name == name && c.IsActive);
            }
        }

        public async Task<bool> CheckCodeByIdExistenceAsync(int id)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                return await context.PromoCodes
                    .AnyAsync(code => code.Id == id);
            }
        }

        public async Task DeleteAsync(PromoCode code)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                context.PromoCodes.Remove(code);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<PromoCode>> GetAllCodesAsync()
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                return await context.PromoCodes
                    .Where(code => code.NumberOfPossibleUses > 0 && code.IsActive)
                    .ToListAsync();
            }
        }

        public async Task<PromoCode?> GetCodeByIdAsync(int id)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                return await context.PromoCodes
                    .FirstOrDefaultAsync(code => code.Id == id & code.NumberOfPossibleUses > 0 && code.IsActive);
            }
        }

        public async Task<PromoCode> GetCodeByIdWithTransactionAsync(int id)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                using (var transaction = context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var code = await context.PromoCodes.FirstOrDefaultAsync(code => code.Id == id && code.NumberOfPossibleUses > 0 && code.IsActive);
                        if (code != null)
                        {
                            code.NumberOfPossibleUses -= 1;
                            await context.SaveChangesAsync();

                            await _changeLogServices.AddChangeLogAsync(context, "PromoCode", "PC downloaded", code);

                            await transaction.Result.CommitAsync();
                        }
                        return code;
                    }
                    catch (Exception ex)
                    {
                        await transaction.Result.RollbackAsync();
                        throw new Exception($"Wystąpił błąd podczas pobierania kodu promocyjnego: {ex.Message}");
                    }
                }
            }
        }

        public async Task SetPromoCodeToInactiveAsync(int id)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                var promoCode = context.PromoCodes.FirstOrDefault(code => code.Id == id);

                if (promoCode != null)
                {
                    promoCode.IsActive = false;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task UpdatePromoCodeNameAsync(int id, string newName)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                var promoCode = context.PromoCodes.FirstOrDefault(code => code.Id == id);

                if (promoCode != null)
                {
                    promoCode.Name = newName;
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
