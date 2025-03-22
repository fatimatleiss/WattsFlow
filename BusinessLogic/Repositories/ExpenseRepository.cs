using Microsoft.EntityFrameworkCore;
using WattsFlowProject.Data;
using WattsFlowProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace WattsFlowProject.BusinessLogic.Repositories
{
    public class ExpenseRepository
    {
        private readonly WattsFlowDbContext _context;

        public ExpenseRepository(WattsFlowDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExpenseType>> GetExpenseTypesAsync()
        {
            try
            {
                return await _context.Types.Where(x => x.IsDeleted != true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving expense types: " + ex.Message);
            }
        }

        public async Task<List<Expenses>> GetExpensesAsync()
        {
            try
            {
                return await _context.Expenses
                                     .Include(e => e.ExpenseType)
                                     .Where(e => !e.IsDeleted) 
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving expenses: " + ex.Message);
            }
        }

        public async Task CreateExpenseAsync(Expenses expense)
        {
            try
            {
                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while saving expense: " + ex.Message);
            }
        }

        public async Task<Expenses> GetExpenseByIdAsync(int id)
        {
            try
            {
                return await _context.Expenses
                    .FirstOrDefaultAsync(e => e.ExpenseId == id && !e.IsDeleted); 
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving expense by id: " + ex.Message);
            }
        }

        public async Task<bool> EditExpenseAsync(Expenses updatedExpense)
        {
            try
            {
                 var existingExpense = await _context.Expenses
                    .FirstOrDefaultAsync(e => e.ExpenseId == updatedExpense.ExpenseId && !e.IsDeleted);

                if (existingExpense == null)
                {
                    return false; 
                }

                 existingExpense.ExpenseDate = updatedExpense.ExpenseDate;
                existingExpense.ExpenseType = updatedExpense.ExpenseType;
                existingExpense.Amount = updatedExpense.Amount;

                _context.Expenses.Update(existingExpense);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating expense: " + ex.Message);
            }
        }

        public async Task<bool> DeleteExpenseAsync(int expenseId)
        {
            try
            {
                 var expenseToDelete = await _context.Expenses
                    .FirstOrDefaultAsync(e => e.ExpenseId == expenseId && !e.IsDeleted); // Only consider non-deleted records

                if (expenseToDelete == null)
                {
                    return false; 
                }

                 expenseToDelete.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while soft deleting expense: " + ex.Message);
            }
        }
        public async Task<decimal> GetTotalExpensesAsync()
        {
            try
            {
                return await _context.Expenses
                                     .Where(e => !e.IsDeleted) 
                                     .SumAsync(e => e.Amount); 
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while calculating total expenses: " + ex.Message);
            }
        }


        public async Task<decimal> GetTotalDieselExpensesAsync()
        {
            try
            {
                var dieselTypeIds = await _context.Types
                    .Where(et => EF.Functions.Like(et.ExpenseTypeName, "Diesel") ||
                                 EF.Functions.Like(et.ExpenseTypeName, "مازوت"))
                    .Select(et => et.ExpensesTypeId)
                    .ToListAsync();

                if (!dieselTypeIds.Any())
                    return 0;

                return await _context.Expenses
                    .Where(e => dieselTypeIds.Contains(e.ExpensesTypeId) && !e.IsDeleted)
                    .SumAsync(e => e.Amount);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving total diesel expenses: " + ex.Message);
            }
        }
    


    }
}
