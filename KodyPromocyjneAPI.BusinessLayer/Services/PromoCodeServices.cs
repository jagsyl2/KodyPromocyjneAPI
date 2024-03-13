using KodyPromocyjneAPI.DataLayer;
using KodyPromocyjneAPI.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace KodyPromocyjneAPI.BusinessLayer.Services
{
    public interface  IPromoCodeServices
    {
        Task AddAsync(PromoCode code);
        Task DeleteAsync(PromoCode promoCode);
        Task<PromoCode> GetCodeByName(string name);
        Task UpdatePromoCodeNameAsync(int id, string newName);
        Task SetPromoCodeToInactive(int id);
    }

    public class PromoCodeServices : IPromoCodeServices
    {
        private readonly Func<IPromoCodesDbContext> _promoCodesDbContextFactoryMethod;

        public PromoCodeServices(Func<IPromoCodesDbContext> promoCodesDbContextFactoryMethod)
        {
            _promoCodesDbContextFactoryMethod = promoCodesDbContextFactoryMethod;
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

        public async Task DeleteAsync(PromoCode code)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                context.PromoCodes.Remove(code);
                await context.SaveChangesAsync();
            }
        }

        public async Task<PromoCode> GetCodeByName(string name)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                var code = await context.PromoCodes
                    .Where(code => code.Name == name && code.IsActive && code.NumberOfPossibleUses > 0)
                    .AsQueryable()
                    .FirstOrDefaultAsync();

                if(code != null)
                {
                    code.NumberOfPossibleUses -= 1;
                    UpdateAsync(code);
                }

                return code;
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

        public async Task SetPromoCodeToInactive(int id)
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

        private async Task UpdateAsync(PromoCode code)
        {
            using (var context = _promoCodesDbContextFactoryMethod())
            {
                context.PromoCodes.Update(code);
                await context.SaveChangesAsync();
            }
        }
    }
}
