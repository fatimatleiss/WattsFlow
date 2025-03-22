using Microsoft.AspNetCore.Mvc;
using WattsFlowProject.BusinessLogic.Repositories;
using WattsFlowProject.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WattsFlowProject.Controllers
{
    [Authorize]
    public class CustomerTrafficController : Controller
    {
        private readonly CustomerTrafficRepository _trafficRepository;

        public CustomerTrafficController(CustomerTrafficRepository trafficRepository)
        {
            _trafficRepository = trafficRepository;
        }

        public async Task<IActionResult> Index()
        {
            var traffics = await _trafficRepository.GetCustomerTrafficsAsync();
            return View(traffics);
        }

        public async Task<IActionResult> Details(int customerId)
        {
            var traffics = await _trafficRepository.GetCustomerTrafficsByCustomerIdAsync(customerId);
            if (traffics == null || traffics.Count == 0)
            {
                return NotFound();
            }
            return View(traffics);
        }

        public async Task<IActionResult> Create(int customerId)
        {
            var lastTraffic = await _trafficRepository.GetLastTrafficByCustomerIdAsync(customerId);
            var traffic = new CustomerTraffic
            {
                CustomerId = customerId,
                PreviousTrafficDate = lastTraffic?.CurrentTrafficDate ?? DateTime.Now // Default to today if no previous traffic exists
            };

            return View(traffic);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CustomerTraffic traffic)
        {
            if (ModelState.IsValid)
            {
                 if (!_trafficRepository.CanAddTraffic(traffic.CustomerId, traffic.CurrentTrafficDate))
                {
                    TempData["ErrorMessage"] = "تم إدخال حركة مرور لهذا الشهر بالفعل. يُسمح بإدخال 1 فقط لكل شهر.";
                    return View(traffic);
                }

                try
                {
                    await _trafficRepository.CreateCustomerTrafficAsync(traffic);
                    TempData["SuccessMessage"] = "تم إنشاء سجل الحركة بنجاح!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return View(traffic);
                }
            }

             TempData.Remove("ErrorMessage");
            TempData.Remove("SuccessMessage");

            return View(traffic);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var traffic = await _trafficRepository.GetCustomerTrafficByIdAsync(id);
            if (traffic == null)
            {
                return NotFound();
            }
            return View(traffic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerTraffic traffic)
        {
            if (id != traffic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _trafficRepository.EditCustomerTrafficAsync(traffic);
                    if (!result)
                    {
                        return NotFound();
                    }
                    TempData["SuccessMessage"] = "Traffic record updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(traffic);
                }
            }
            return View(traffic);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var traffic = await _trafficRepository.GetCustomerTrafficByIdAsync(id);
            if (traffic == null)
            {
                return NotFound();
            }
            return View(traffic);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _trafficRepository.DeleteCustomerTrafficAsync(id);
            if (!result)
            {
                return NotFound();
            }
            TempData["SuccessMessage"] = "Traffic record deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        //public IActionResult TrafficByMonth()
        //{
        //    return View(new TrafficByMonthViewModel());
        //}

        //// POST: Handle form submission and display the table
        //[HttpPost]
        //public async Task<IActionResult> TrafficByMonth(TrafficByMonthViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var trafficRecords = await _trafficRepository.GetCustomerTrafficByMonthAsync(model.Year, model.Month);
        //        model.TrafficRecords = trafficRecords;
        //    }

        //    return View(model);
        //}

    }
}