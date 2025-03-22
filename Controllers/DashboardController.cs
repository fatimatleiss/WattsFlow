using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WattsFlowProject.BusinessLogic.Repositories;
using System;
using System.Threading.Tasks;

namespace WattsFlowProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ExpenseRepository _expenseRepository;
        private readonly CustomerRepository _customerRepository;


        public DashboardController(ExpenseRepository expenseRepository, CustomerRepository customerRepository)
        {
            _expenseRepository = expenseRepository;
            _customerRepository = customerRepository;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                 decimal totalExpenses = await _expenseRepository.GetTotalExpensesAsync();
                string formattedTotalExpenses = totalExpenses.ToString("#,##0"); // Format it
                ViewData["TotalExpenses"] = formattedTotalExpenses;

                 int inactiveCustomersCount = await _customerRepository.CountInactiveCustomersAsync();
                ViewBag.InactiveCustomersCount = inactiveCustomersCount;

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while retrieving data: " + ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var totalDieselExpenses = await _expenseRepository.GetTotalDieselExpensesAsync();
                ViewBag.TotalDieselExpenses = totalDieselExpenses.ToString("#,##0"); // Format it
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while retrieving total diesel expenses: " + ex.Message;
                return View();
            }
        }
    }
}
