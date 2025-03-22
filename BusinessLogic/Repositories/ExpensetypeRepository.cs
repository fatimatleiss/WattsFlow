using Microsoft.EntityFrameworkCore;
using WattsFlowProject.Data;
using WattsFlowProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WattsFlowProject.BusinessLogic.Repositories
{
    public class ExpensetypeRepository
    {
        private readonly WattsFlowDbContext _context;

        public ExpensetypeRepository(WattsFlowDbContext context)
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

        public async Task CreateExpensetypeAsync(ExpenseType types)
        {
            try
            {
                _context.Types.Add(types);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while saving expense: " + ex.Message);
            }
        }
        public async Task<bool> EditExpenseTypeAsync(ExpenseType updatedExpenseType)
        {
            try
            {
                var existingExpenseType = await _context.Types
                    .FirstOrDefaultAsync(e => e.ExpensesTypeId == updatedExpenseType.ExpensesTypeId);

                if (existingExpenseType == null)
                {
                    return false;
                }
                existingExpenseType.ExpenseTypeName = updatedExpenseType.ExpenseTypeName;
                _context.Types.Update(existingExpenseType);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating expense type: " + ex.Message);
            }
        }//
        public async Task<bool> DeleteExpenseTypeAsync(int expenseTypeId)
        {
            try
            {
                var expenseTypeToDelete = await _context.Types
                    .FirstOrDefaultAsync(e => e.ExpensesTypeId == expenseTypeId);

                if (expenseTypeToDelete == null)
                {
                    return false;
                }

                _context.Types.Remove(expenseTypeToDelete);
                await _context.SaveChangesAsync();
                return true;
            }//
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting expense type: " + ex.Message);
            }
        }
    }
}
