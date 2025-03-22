using Microsoft.EntityFrameworkCore;
using WattsFlowProject.Data;
using WattsFlowProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WattsFlowProject.BusinessLogic.Repositories
{
    public class CustomerRepository
    {

        private readonly WattsFlowDbContext _context;

        public CustomerRepository(WattsFlowDbContext context)
        {
            _context = context;
        }
      
        public async Task<List<PostDetails>> GetPostDetailsAsync()
        {
            try
            {
                return await _context.Details.Where(x => x.IsDeleted != true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving post numbers: " + ex.Message);
            }
        }
       

        public async Task<List<Customer>> GetCustomersAsync()
        {
            try
            {
                 return await _context.Customers
                                     .Include(e => e.PostDetails)
                                     .Where(e => !e.IsDeleted) 
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving customers: " + ex.Message);
            }
        }

        public async Task CreateCustomersAsync(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while saving customers: " + ex.Message);
            }
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            try
            {
                 return await _context.Customers
                    .FirstOrDefaultAsync(e => e.CustomerId == id && !e.IsDeleted); 
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving customer by id: " + ex.Message);
            }
        }

        public async Task<bool> EditCustomerAsync(Customer updatedCustomer)
        {
            try
            {
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(e => e.CustomerId == updatedCustomer.CustomerId && !e.IsDeleted);

                if (existingCustomer == null)
                {
                    return false;
                }
                existingCustomer.FirstName = updatedCustomer.FirstName;
                existingCustomer.FatherName = updatedCustomer.FatherName;
                existingCustomer.LastName = updatedCustomer.LastName;
                existingCustomer.PhoneNumber = updatedCustomer.PhoneNumber;
                existingCustomer.Address = updatedCustomer.Address;
                existingCustomer.Building = updatedCustomer.Building;
                existingCustomer.Floor = updatedCustomer.Floor;
                existingCustomer.SideType = updatedCustomer.SideType;
                existingCustomer.PostId = updatedCustomer.PostId;
                existingCustomer.CircuitBreakerPower = updatedCustomer.CircuitBreakerPower;
                existingCustomer.FixedPayment = updatedCustomer.FixedPayment;
                existingCustomer.IsActive = updatedCustomer.IsActive; // Ensure IsActive is updated

                _context.Customers.Update(existingCustomer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating customer: " + ex.Message);
            }
        }
        public async Task<List<Customer>> GetCustomersByPostNumberAsync(int postNumber)
        {
            return await _context.Customers
                .Where(c => c.PostId == postNumber)
                .ToListAsync();
        }


        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            try
            {
                var CustomerToDelete = await _context.Customers
                    .FirstOrDefaultAsync(e => e.CustomerId == customerId && !e.IsDeleted); 

                if (CustomerToDelete == null)
                {
                    return false; 
                }

                CustomerToDelete.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while soft deleting expense: " + ex.Message);
            }
        }
        public async Task<int> CountInactiveCustomersAsync()
        {
            return await _context.Customers.CountAsync(c => !c.IsActive);
        }

    }
}