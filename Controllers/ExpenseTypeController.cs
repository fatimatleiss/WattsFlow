using Microsoft.AspNetCore.Mvc;
using WattsFlowProject.Models;
using WattsFlowProject.BusinessLogic.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WattsFlowProject.Controllers
{
    [Authorize]

    public class ExpenseTypeController : BaseController
    {
        private readonly ExpensetypeRepository _expensetypeRepository;


        public ExpenseTypeController(ExpensetypeRepository expensetypeRepository, SystemSettingsRepository settingsRepository)
            : base(settingsRepository)
        {
            _expensetypeRepository = expensetypeRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var types = await _expensetypeRepository.GetExpenseTypesAsync();
                return View(types);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error occurred while fetching types: " + ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Create()
        {
            return View(new ExpenseType()); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseType expenseType)
        {
            if (!ModelState.IsValid)
            {
                 ViewBag.ExpenseTypes = await _expensetypeRepository.GetExpenseTypesAsync();
                return View(expenseType);
            }

            var existingType = (await _expensetypeRepository.GetExpenseTypesAsync())
                .FirstOrDefault(it => it.ExpenseTypeName?.ToLower() == expenseType.ExpenseTypeName?.ToLower());

            if (existingType != null)
            {
                ModelState.AddModelError("ExpenseTypeName", "This expense type already exists.");
                return View(expenseType);
            }

            try
            {
                expenseType.IsDeleted = false;
                await _expensetypeRepository.CreateExpensetypeAsync(expenseType);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error occurred while saving the expense type: " + ex.Message;
                return View(expenseType);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var expenseType = (await _expensetypeRepository.GetExpenseTypesAsync())
                .FirstOrDefault(e => e.ExpensesTypeId == id);

            if (expenseType == null)
            {
                return NotFound();
            }

            return View(expenseType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExpenseType expenseType)
        {
            if (id != expenseType.ExpensesTypeId)
            {
                ModelState.AddModelError("ExpensesTypeId", "Invalid Expense Type ID.");
                return View(expenseType);
            }

            if (!ModelState.IsValid)
            {
                return View(expenseType);
            }

            var existingType = (await _expensetypeRepository.GetExpenseTypesAsync())
                .FirstOrDefault(et => et.ExpenseTypeName?.ToLower() == expenseType.ExpenseTypeName?.ToLower() && et.ExpensesTypeId != expenseType.ExpensesTypeId);

            if (existingType != null)
            {
                ModelState.AddModelError("ExpenseTypeName", "This type already exists.");
                return View(expenseType);
            }

            try
            {
                bool isUpdated = await _expensetypeRepository.EditExpenseTypeAsync(expenseType);
                if (!isUpdated)
                {
                    ModelState.AddModelError("ExpensesTypeId", "Expense Type not found.");
                    return View(expenseType);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the expense type: " + ex.Message);
                return View(expenseType);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool isDeleted = await _expensetypeRepository.DeleteExpenseTypeAsync(id);
                if (!isDeleted)
                {
                    ViewBag.ErrorMessage = "Expense type not found or failed to delete.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error occurred while deleting the expense type: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
