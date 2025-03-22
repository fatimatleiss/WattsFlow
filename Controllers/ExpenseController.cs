using Microsoft.AspNetCore.Mvc;
using WattsFlowProject.Models;
using WattsFlowProject.ViewModels;
using WattsFlowProject.BusinessLogic.Repositories; 
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WattsFlowProject.Controllers
{
    [Authorize]

    public class ExpenseController : Controller
    {
        private readonly ExpenseRepository _expenseRepository;

        public ExpenseController(ExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var expenses = await _expenseRepository.GetExpensesAsync();
                ViewBag.TotalDieselExpenses = await _expenseRepository.GetTotalDieselExpensesAsync();
                return View(expenses);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error occurred while fetching expenses: " + ex.Message;
                return View();
            }
        }


        public async Task<IActionResult> Create()
        {
            try
            {
                var expenseTypes = await _expenseRepository.GetExpenseTypesAsync();
                var viewModel = new ExpensesViewModel
                {
                    types = expenseTypes
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error occurred while loading the create form: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpensesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var expense = new Expenses
                    {
                        ExpenseDate = viewModel.ExpenseDate,
                        ExpensesTypeId = viewModel.ExpensesTypeId,
                        Amount = viewModel.Amount
                    };

                    await _expenseRepository.CreateExpenseAsync(expense);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error occurred while saving the expense: " + ex.Message;
                }
            }
            else
            {
                var expenseTypes = await _expenseRepository.GetExpenseTypesAsync();
                viewModel.types = expenseTypes;
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var expenses = await _expenseRepository.GetExpensesAsync();
                var expenseToEdit = expenses.FirstOrDefault(e => e.ExpenseId == id);

                if (expenseToEdit == null)
                {
                    return NotFound(); 
                }

                var expenseTypes = await _expenseRepository.GetExpenseTypesAsync();
                var viewModel = new ExpensesViewModel
                {
                    ExpenseId = expenseToEdit.ExpenseId,
                    ExpenseDate = expenseToEdit.ExpenseDate,
                    ExpensesTypeId = expenseToEdit.ExpensesTypeId,
                    Amount = Math.Round(expenseToEdit.Amount, 3),
                    types = expenseTypes
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error occurred while loading the edit form: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ExpensesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                     var existingExpense = await _expenseRepository.GetExpenseByIdAsync(viewModel.ExpenseId);

                    if (existingExpense == null)
                    {
                        ViewBag.ErrorMessage = "Expense not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    existingExpense.ExpenseDate = viewModel.ExpenseDate;
                    existingExpense.ExpensesTypeId = viewModel.ExpensesTypeId;
                    existingExpense.Amount = viewModel.Amount;

                     bool isUpdated = await _expenseRepository.EditExpenseAsync(existingExpense);

                    if (isUpdated)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to update the expense.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error occurred while updating the expense: " + ex.Message;
                }
            }

             var expenseTypes = await _expenseRepository.GetExpenseTypesAsync();
            viewModel.types = expenseTypes;
            return View(viewModel);
        }
      
        public async Task<IActionResult> DieselExpenses()
        {
            var totalDieselExpenses = await _expenseRepository.GetTotalDieselExpensesAsync();
            ViewBag.TotalDieselExpenses = totalDieselExpenses;
            return View();
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                 var isDeleted = await _expenseRepository.DeleteExpenseAsync(id);

                if (!isDeleted)
                {
                    ViewBag.ErrorMessage = "Expense not found or already deleted.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error occurred while deleting the expense: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


    }
}
