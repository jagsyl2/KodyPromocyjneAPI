using Microsoft.AspNetCore.Mvc;

namespace KodyPromocyjneAPI.Controllers
{
    [Route("api/status")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public string GetStatus()
        {
            return "Status OK";
        }
    }
}