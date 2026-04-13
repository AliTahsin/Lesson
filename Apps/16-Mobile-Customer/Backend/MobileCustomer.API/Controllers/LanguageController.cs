using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileCustomer.API.DTOs;
using MobileCustomer.API.Services;

namespace MobileCustomer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet("supported")]
        public async Task<IActionResult> GetSupportedLanguages()
        {
            var languages = await _languageService.GetSupportedLanguagesAsync();
            return Ok(languages);
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentLanguage()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var language = await _languageService.GetUserLanguageAsync(userId);
            return Ok(new { language });
        }

        [HttpPut("current")]
        public async Task<IActionResult> SetLanguage([FromBody] SetLanguageDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var result = await _languageService.SetUserLanguageAsync(userId, dto.LanguageCode);
            return Ok(new { success = result });
        }

        [HttpGet("translations/{languageCode}")]
        public async Task<IActionResult> GetTranslations(string languageCode)
        {
            var translations = await _languageService.GetTranslationsAsync(languageCode);
            return Ok(translations);
        }
    }

    public class SetLanguageDto
    {
        public string LanguageCode { get; set; }
    }
}