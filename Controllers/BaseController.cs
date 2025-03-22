using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using WattsFlowProject.BusinessLogic.Repositories;

namespace WattsFlowProject.Controllers
{

    public class BaseController : Controller
    {
        private readonly SystemSettingsRepository _settingsRepository;

        public BaseController(SystemSettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
             var settings = await _settingsRepository.GetSystemSettingsAsync();
            ViewData["SystemTitle"] = settings?.Title ?? "لوحة التحكم الإدارية"; 

             await next();
        }
    }
}
