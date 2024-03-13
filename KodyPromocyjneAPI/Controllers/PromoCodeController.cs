using KodyPromocyjneAPI.BusinessLayer.Services;
using KodyPromocyjneAPI.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace KodyPromocyjneAPI.Controllers
{
    [Route("api/promocode")]
    public class PromoCodeController : ControllerBase
    {
        private readonly IPromoCodeServices _promoCodeServices;
        private readonly IChangeLogServices _changeLogServices;

        public PromoCodeController(
            IPromoCodeServices promoCodeServices, 
            IChangeLogServices changeLogServices)
        {
            _promoCodeServices = promoCodeServices;
            _changeLogServices = changeLogServices;
        }

        [HttpPost]
        public async Task PostPromoCode([FromBody] PromoCode code)
        {
            try
            {
                await _promoCodeServices.AddAsync(code);
                await _changeLogServices.AddChangeLogAsync(new ChangeLog
                {
                    TableName = "PromoCode",
                    Description = "Promo code added",
                    CodeId = code.Id,
                    NewValue = code.Name,
                    DateChanged = DateTime.Now,
                });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("{id}/inactive")]
        public async Task SetPromoCodeToInactive(int id)
        {
            try
            {
                await _promoCodeServices.SetPromoCodeToInactive(id);
                await _changeLogServices.AddChangeLogAsync(new ChangeLog
                {
                    TableName = "PromoCode",
                    Description = "PC set to inactive",
                    CodeId = id,
                    NewValue = "false",
                    DateChanged = DateTime.Now,
                });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("{id}/changeName")]
        public async Task ChangeNameOfPromoCode(int id, [FromBody] string newName)
        {
            try
            {
                await _promoCodeServices.UpdatePromoCodeNameAsync(id, newName);
                await _changeLogServices.AddChangeLogAsync(new ChangeLog
                {
                    TableName = "PromoCode",
                    Description = "PC Name changed",
                    CodeId = id,
                    NewValue = newName,
                    DateChanged = DateTime.Now,
                });

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("{name}")]
        public async Task<PromoCode> GetPromoCodeByName(string name)
        {
            try
            {
                var code = await _promoCodeServices.GetCodeByName(name);
                if (code != null)
                {
                    await _changeLogServices.AddChangeLogAsync(new ChangeLog
                    {
                        TableName = "PromoCode",
                        Description = "PC downloaded",
                        CodeId = code.Id,
                        NewValue = code.NumberOfPossibleUses.ToString(),
                        DateChanged = DateTime.Now,
                    });
                }
                return code;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task DeletePromoCode(int id)
        {
            try
            {
                await _promoCodeServices.DeleteAsync(new PromoCode
                {
                    Id = id
                });
                await _changeLogServices.AddChangeLogAsync(new ChangeLog
                {
                    TableName = "PromoCode",
                    Description = "PC deleted",
                    CodeId = id,
                    NewValue = "null",
                    DateChanged = DateTime.Now,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
