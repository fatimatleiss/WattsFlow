using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WattsFlowProject.Data;
using WattsFlowProject.Models;

namespace WattsFlowProject.Services
{
    public class CustomerService
    {
        private readonly WattsFlowDbContext _context;

        public CustomerService(WattsFlowDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddCustomerAsync(Customer customer)
        {
            // Get the number of customers for the given PostId
            var customerCount = await _context.Customers
                .Where(c => c.PostId == customer.PostId) // Count by PostId (not PostNumber)
                .CountAsync();

            // Define the maximum number of customers per post
            const int maxCustomersPerPost = 24;

            // Check if the number of customers exceeds the limit
            if (customerCount >= maxCustomersPerPost)
            {
                return "Cannot add more customers to this Post. The limit has been reached.";
            }

            // Otherwise, add the customer
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return "Customer added successfully.";
        }
    }
}
