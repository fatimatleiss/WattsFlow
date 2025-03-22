using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WattsFlowProject.Models;
using WattsFlowProject;
using WattsFlowProject.BusinessLogic.Repositories;
namespace WattsFlowProject.Controllers
{
    [Authorize]
    public class SystemSettingsController : BaseController
    {
        private readonly SystemSettingsRepository _settingsRepository;

        public SystemSettingsController(SystemSettingsRepository settingsRepository):base(settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<IActionResult> Index()
        {
             ViewData["SuccessMessage"] = TempData["SuccessMessage"] as string;
            TempData.Remove("SuccessMessage");

            var settings = await _settingsRepository.GetSystemSettingsAsync();
            ViewData["SystemTitle"] = settings?.Title ?? "WattsFlowProject";  

            if (settings == null)
            {
                settings = new SystemSettings(); 
            }

            return View(settings);
        }
        [HttpPost]
        public async Task<IActionResult> Index(SystemSettings settings)
        {
            if (ModelState.IsValid)
            {
                await _settingsRepository.UpdateSystemSettingsAsync(settings);
                TempData["SuccessMessage"] = "تم تحديث الإعدادات بنجاح.";
                return RedirectToAction(nameof(Index));
            }
            return View(settings);
        }
    }
}