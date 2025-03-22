using Microsoft.AspNetCore.Mvc;
using WattsFlowProject.Models;
using WattsFlowProject.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using WattsFlowProject.BusinessLogic.Repositories;

namespace WattsFlowProject.Controllers
{
    [Authorize]

    public class CustomerController : Controller
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {


                var customers = await _customerRepository.GetCustomersAsync();
                return View(customers);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "حدث خطأ أثناء جلب البيانات للعملاء: " + ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var postDetails = await _customerRepository.GetPostDetailsAsync();

                var viewModel = new PostCustomerViewModel
                {
                    posts = postDetails.Select(p => new SelectListItem
                    {
                        Value = p.PostId.ToString(),
                        Text = p.PostNumber.ToString()
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "حدث خطأ أثناء تحميل نموذج الإنشاء: " + ex.Message;
                return View(new PostCustomerViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostCustomerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine($"تم اختيار رقم البوست: {viewModel.PostId}");
                    var existingCustomersWithPost = await _customerRepository.GetCustomersByPostNumberAsync(viewModel.PostId);

                    if (existingCustomersWithPost.Count >= 26)
                    {
                        ViewBag.ErrorMessage = "هذا الرقم البوست يحتوي على 24 عميل بالفعل. من فضلك اختر رقم بوست آخر.";

                        return View(viewModel);
                    }

                    var customer = new Customer
                    {
                        FirstName = viewModel.FirstName,
                        FatherName = viewModel.FatherName,
                        LastName = viewModel.LastName,
                        PhoneNumber = viewModel.PhoneNumber,
                        Address = viewModel.Address,
                        Building = viewModel.Building,
                        Floor = viewModel.Floor,
                        SideType = viewModel.Side,
                        PostId = viewModel.PostId,
                        CircuitBreakerPower = viewModel.CircuitBreakerPower,
                        FixedPayment = viewModel.FixedPayment,
                        IsActive = viewModel.IsActive, // Ensure IsActive is set
                        AddedDate = DateTime.UtcNow
                    };

                    await _customerRepository.CreateCustomersAsync(customer);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "حدث خطأ أثناء حفظ العميل: " + ex.Message;
                }
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                var postDetails = await _customerRepository.GetPostDetailsAsync();
                var viewModel = new PostCustomerViewModel
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    FatherName = customer.FatherName,
                    LastName = customer.LastName,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address,
                    Building = customer.Building,
                    Floor = customer.Floor,
                    Side = customer.SideType,
                    PostId = customer.PostId,
                    CircuitBreakerPower = customer.CircuitBreakerPower,
                    FixedPayment = customer.FixedPayment,
                    IsActive = customer.IsActive,  
                    AddedDate = DateTime.UtcNow,
                    posts = postDetails.Select(p => new SelectListItem
                    {
                        Value = p.PostId.ToString(), 
                        Text = p.PostNumber.ToString() 
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "حدث خطأ أثناء تحميل نموذج التعديل: " + ex.Message;
                return View(new PostCustomerViewModel());
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(PostCustomerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var postDetails = await _customerRepository.GetPostDetailsAsync();
                viewModel.posts = postDetails.Select(p => new SelectListItem
                {
                    Value = p.PostId.ToString(),
                    Text = p.PostNumber.ToString()
                }).ToList();

                return View(viewModel);
            }

            try
            {
                var existingCustomer = await _customerRepository.GetCustomerByIdAsync(viewModel.CustomerId);
                if (existingCustomer == null)
                {
                    ViewBag.ErrorMessage = "لم يتم العثور على العميل.";
                    return RedirectToAction(nameof(Index));
                }
                existingCustomer.FirstName = viewModel.FirstName;
                existingCustomer.FatherName = viewModel.FatherName;
                existingCustomer.LastName = viewModel.LastName;
                existingCustomer.PhoneNumber = viewModel.PhoneNumber;
                existingCustomer.Address = viewModel.Address;
                existingCustomer.Building = viewModel.Building;
                existingCustomer.Floor = viewModel.Floor;
                existingCustomer.SideType = viewModel.Side;
                existingCustomer.PostId = viewModel.PostId;
                existingCustomer.CircuitBreakerPower = viewModel.CircuitBreakerPower;
                existingCustomer.FixedPayment = viewModel.FixedPayment;
                existingCustomer.IsActive = viewModel.IsActive; 

                bool isUpdated = await _customerRepository.EditCustomerAsync(existingCustomer);

                if (isUpdated)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMessage = "فشل في تحديث العميل.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "حدث خطأ أثناء تحديث العميل: " + ex.Message;
            }

            var postList = await _customerRepository.GetPostDetailsAsync();
            viewModel.posts = postList.Select(p => new SelectListItem
            {
                Value = p.PostId.ToString(),
                Text = p.PostNumber.ToString()
            }).ToList();

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var isDeleted = await _customerRepository.DeleteCustomerAsync(id);

                if (!isDeleted)
                {
                    ViewBag.ErrorMessage = "العميل غير موجود أو تم حذفه بالفعل.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "حدث خطأ أثناء حذف العميل: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
