using KodyPromocyjneAPI.BusinessLayer.Services;
using KodyPromocyjneAPI.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace KodyPromocyjneAPI.Controllers
{
    [ApiController]
    [Route("api/promocodes")]
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
        public async Task<ActionResult> PostPromoCode([FromBody] PromoCode code)
        {
            try
            {
                var codeExist = await _promoCodeServices.CheckActiveCodeExistenceAsync(code);
                if (codeExist)
                {
                    ModelState.AddModelError("Name", "Kod promocyjny o tej nazwie już istnieje i jest aktywny");
                    return BadRequest(ModelState);
                }

                await _promoCodeServices.AddAsync(code);
                await _changeLogServices.AddChangeLogAsync("PromoCode", "Promo code added", code.Id, code.Name);

                return Ok("Kod promocyjny został dodany");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas dodawania kodu promocyjnego: {ex.Message}");
            }
        }

        [HttpPut("inactive/{id}")]
        public async Task<ActionResult> SetPromoCodeToInactive(int id)
        {
            try
            {
                var code = await _promoCodeServices.GetCodeByIdAsync(id);
                if (code == null)
                {
                    return NotFound($"Kod promocyjny o Id = {id} nie został znaleziony");
                }

                await _promoCodeServices.SetPromoCodeToInactiveAsync(id);
                await _changeLogServices.AddChangeLogAsync("PromoCode", "PC set to inactive", id, "false");

                return Ok($"Kod promocyjny {id}. {code.Name} został ustawiony jako nieaktywny");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas ustawiania kodu promocyjnego jako niekatywny: {ex.Message}");
            }
        }

        [HttpPut("changeName/{id}")]
        public async Task<ActionResult> ChangeNameOfPromoCode(int id, [FromBody] string newName)
        {
            try
            {
                var code = await _promoCodeServices.GetCodeByIdAsync(id);
                if (code == null)
                {
                    return NotFound($"Kod promocyjny o Id = {id} nie został znaleziony");
                }
                
                var nameExist = await _promoCodeServices.CheckActiveCodeExistenceAsync(newName);
                if(nameExist)
                {
                    ModelState.AddModelError("Name", "Kod promocyjny o tej nazwie już istnieje i jest aktywny");
                    return BadRequest(ModelState);
                }

                await _promoCodeServices.UpdatePromoCodeNameAsync(id, newName);
                await _changeLogServices.AddChangeLogAsync("PromoCode", "PC Name changed", id, newName);

                return Ok($"Nazwa kodu promocyjnego o Id = {id} została zmieniona z {code.Name} na {newName}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas zmiany nazwy kodu promocyjnego: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetPromoCodes()
        {
            try
            {
                return Ok(await _promoCodeServices.GetAllCodesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas pobierania kodów promocyjnych: {ex.Message}");
            }
        }

        [HttpGet("display/{id}")]
        public async Task<ActionResult<PromoCode>> GetPromoCodeById(int id)
        {
            try
            {
                var code = await _promoCodeServices.GetCodeByIdWithTransactionAsync(id);
                if (code == null)
                {
                    return NotFound($"Kod promocyjny o Id = {id} nie został znaleziony");
                }

                return Ok(code);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas pobierania kodu promocyjnego: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePromoCode(int id)
        {
            try
            {
                var codeExist = await _promoCodeServices.CheckCodeByIdExistenceAsync(id);
                if (!codeExist)
                {
                    return NotFound($"Kod promocyjny o Id = {id} nie został znaleziony");
                }

                await _promoCodeServices.DeleteAsync(new PromoCode { Id = id });
                await _changeLogServices.AddChangeLogAsync("PromoCode", "PC deleted", id, "null");

                return Ok($"Kod promocyjny o Id = {id} został usunięty");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas usuwania kodu promocyjnego: {ex.Message}");
            }
        }
    }
}
