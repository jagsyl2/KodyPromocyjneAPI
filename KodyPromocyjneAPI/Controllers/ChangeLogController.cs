using KodyPromocyjneAPI.BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace KodyPromocyjneAPI.Controllers
{
    [ApiController]
    [Route("api/changelogs")]
    public class ChangeLogController : ControllerBase
    {
        private readonly IChangeLogServices _changeLogServices;

        public ChangeLogController(IChangeLogServices changeLogServices)
        {
            _changeLogServices = changeLogServices;
        }

        [HttpGet("find")]
        public async Task<ActionResult> GetChangeLogByPromoCodeIdAsync([FromQuery] int idOfPromoCode)
        {
            try
            {
                var logs = await _changeLogServices.GetChangeLogsByCodeIdAsync(idOfPromoCode);
                if (logs.Count == 0)
                {
                    return NotFound($"Nie znaleziono logów zmian dla kodu promocyjnego o Id = {idOfPromoCode}");
                }

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas pobierania logów zmian: {ex.Message}");
            }
        }
    }
}
