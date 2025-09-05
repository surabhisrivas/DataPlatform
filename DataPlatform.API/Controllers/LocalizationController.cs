using DataPlatform.Application.Interfaces;
using DataPlatform.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DataPlatform.API.Controllers
{
    [Route("localization")]
    public class LocalizationController : BaseController
    {
        private readonly ILocalizationService _localizationService;

        public LocalizationController(ILocalizationService localizationService)
        {
            _localizationService= localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var results =  await _localizationService.GetLocalizationsAsync("en");
            return Ok(results);
        }
    }
}