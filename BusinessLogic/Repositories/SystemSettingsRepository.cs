using Microsoft.EntityFrameworkCore;
using WattsFlowProject.Data;
using WattsFlowProject.Models;

namespace WattsFlowProject.BusinessLogic.Repositories
{
    public class SystemSettingsRepository
    {
        private readonly WattsFlowDbContext _context;

        public SystemSettingsRepository(WattsFlowDbContext context)
        {
            _context = context;
        }

         public async Task<SystemSettings> GetSystemSettingsAsync()
        {
            return await _context.SystemSettings.FirstOrDefaultAsync();
        }

        public async Task UpdateSystemSettingsAsync(SystemSettings settings)
        {
            var existingSettings = await _context.SystemSettings.FirstOrDefaultAsync();
            if (existingSettings != null)
            {
                existingSettings.KwPrice = settings.KwPrice;
                existingSettings.LiraRate = settings.LiraRate;
                existingSettings.Title = settings.Title;
                _context.SystemSettings.Update(existingSettings);
            }
            else
            {
                _context.SystemSettings.Add(settings);
            }

            await _context.SaveChangesAsync();
        }


    }
}