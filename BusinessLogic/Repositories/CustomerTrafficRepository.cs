using Microsoft.EntityFrameworkCore;
using WattsFlowProject.Data;
using WattsFlowProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WattsFlowProject.BusinessLogic.Repositories
{
    public class CustomerTrafficRepository
    {
        private readonly WattsFlowDbContext _context;
        private readonly SystemSettingsRepository _settingsRepository;

        public CustomerTrafficRepository(WattsFlowDbContext context, SystemSettingsRepository settingsRepository)
        {
            _context = context;
            _settingsRepository = settingsRepository;
        }

        public async Task<CustomerTraffic> GetLastTrafficByCustomerIdAsync(int customerId)
        {
            return await _context.Traffics
                .Where(t => t.CustomerId == customerId)
                .OrderByDescending(t => t.CurrentTrafficDate)
                .FirstOrDefaultAsync();
        }

        public bool CanAddTraffic(int customerId, DateTime currentTrafficDate)
        {
            var startOfMonth = new DateTime(currentTrafficDate.Year, currentTrafficDate.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var trafficCount = _context.Traffics
                .Count(t => t.CustomerId == customerId &&
                            t.CurrentTrafficDate >= startOfMonth &&
                            t.CurrentTrafficDate <= endOfMonth &&
                            !t.IsDeleted); 

            return trafficCount < 1;
        }

        public async Task<List<CustomerTraffic>> GetCustomerTrafficsAsync()
        {
            return await _context.Traffics
                .Include(t => t.Customer)
                .Where(t => !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<CustomerTraffic> GetCustomerTrafficByIdAsync(int id)
        {
            return await _context.Traffics
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task<List<CustomerTraffic>> GetCustomerTrafficsByCustomerIdAsync(int customerId)
        {
            return await _context.Traffics
                .Where(t => t.CustomerId == customerId && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<CustomerTraffic>> GetMonthlyTrafficAsync(int month, int year)
        {
            return await _context.Traffics
                .Where(t => t.CurrentTrafficDate.Month == month && t.CurrentTrafficDate.Year == year)
                .Include(t => t.Customer)
                .ToListAsync();
        }

        public async Task CreateCustomerTrafficAsync(CustomerTraffic traffic)
        {
            var customerExists = await _context.Customers
                .AnyAsync(c => c.CustomerId == traffic.CustomerId);

            if (!customerExists)
            {
                throw new InvalidOperationException("The specified CustomerId does not exist in the customers table.");
            }

            var settings = await _settingsRepository.GetSystemSettingsAsync();
            if (settings == null)
            {
                throw new InvalidOperationException("System settings must be configured before adding traffic.");
            }

            var lastTraffic = await _context.Traffics
                .Where(t => t.CustomerId == traffic.CustomerId)
                .OrderByDescending(t => t.CurrentTrafficDate)
                .FirstOrDefaultAsync();

            if (lastTraffic != null)
            {
                if (traffic.CurrentTrafficDate < lastTraffic.CurrentTrafficDate)
                {
                    throw new Exception("The current traffic date cannot be before the previous traffic date.");
                }

                traffic.PreviousTraffic = lastTraffic.CurrentTraffic;
                traffic.PreviousTrafficDate = lastTraffic.CurrentTrafficDate;

                var historyRecord = new CustomerTrafficHistory
                {
                    CustomerId = lastTraffic.CustomerId,
                    PreviousTraffic = lastTraffic.PreviousTraffic,
                    PreviousTrafficDate = lastTraffic.PreviousTrafficDate,
                    CurrentTraffic = lastTraffic.CurrentTraffic,
                    CurrentTrafficDate = lastTraffic.CurrentTrafficDate,
                    Consumption = lastTraffic.Consumption,
                    PriceUSD = lastTraffic.PriceUSD,
                    PriceLBP = lastTraffic.PriceLBP,
                    PaidAmount = lastTraffic.PaidAmount,
                    RemainingAmount = lastTraffic.RemainingAmount,
                    IsDeleted = lastTraffic.IsDeleted
                };

                _context.TrafficHistories.Add(historyRecord);
                await _context.SaveChangesAsync();
            }
            else
            {
                traffic.PreviousTraffic = 0;
                traffic.PreviousTrafficDate = DateTime.Now;
            }

            traffic.Consumption = traffic.CurrentTraffic - traffic.PreviousTraffic;
            traffic.PriceUSD = (decimal)traffic.Consumption * settings.KwPrice;
            traffic.PriceLBP = traffic.PriceUSD * settings.LiraRate;
            traffic.RemainingAmount = traffic.PriceLBP - (traffic.PaidAmount ?? 0);

            _context.Traffics.Add(traffic);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EditCustomerTrafficAsync(CustomerTraffic updatedTraffic)
        {
            var existingTraffic = await _context.Traffics
                .FirstOrDefaultAsync(t => t.Id == updatedTraffic.Id && !t.IsDeleted);

            if (existingTraffic == null)
            {
                return false;
            }

            if (updatedTraffic.CurrentTrafficDate < existingTraffic.PreviousTrafficDate)
            {
                throw new Exception("The current traffic date cannot be before the previous traffic date.");
            }

            existingTraffic.CurrentTraffic = updatedTraffic.CurrentTraffic;
            existingTraffic.PaidAmount = updatedTraffic.PaidAmount;
            existingTraffic.CurrentTrafficDate = updatedTraffic.CurrentTrafficDate;

            var settings = await _settingsRepository.GetSystemSettingsAsync();
            if (settings == null)
            {
                throw new InvalidOperationException("System settings must be configured before updating traffic.");
            }

            existingTraffic.Consumption = existingTraffic.CurrentTraffic - existingTraffic.PreviousTraffic;
            existingTraffic.PriceUSD = (decimal)existingTraffic.Consumption * settings.KwPrice;
            existingTraffic.PriceLBP = existingTraffic.PriceUSD * settings.LiraRate;
            existingTraffic.RemainingAmount = existingTraffic.PriceLBP - (existingTraffic.PaidAmount ?? 0);

            _context.Traffics.Update(existingTraffic);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCustomerTrafficAsync(int id)
        {
            var trafficToDelete = await _context.Traffics
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            if (trafficToDelete == null)
            {
                return false;
            }

            trafficToDelete.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true; 
        }
        public async Task<List<CustomerTraffic>> GetCustomerTrafficByMonthAsync(int year, int month)
        {
             var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

             return await _context.Traffics
                .Where(t => t.CurrentTrafficDate >= startDate && t.CurrentTrafficDate <= endDate)
                .Include(t => t.Customer)
                .ToListAsync();
        }

    }
}