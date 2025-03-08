using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Domin.Entity;
using Infarstuructre.Data;
using Infarstuructre.ViewModel;
using MyService.Resource;

namespace MyService.Controllers
{
    public class RequestController : Controller
    {
        private readonly MyServiceDbContext _context;

        public RequestController(MyServiceDbContext context)
        {
            _context = context;
        }

        // GET: Request/Create
        public async Task<IActionResult> SendRequest()
        {
            var viewModel = new RequestViewModel
            {
                OrderDate = DateTime.Today
            };

            await PopulateDropdownsAsync(viewModel);
            return View(viewModel);
        }

        // POST: Request/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendRequest(RequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var request = new Request
                {
                    CustomerId = viewModel.CustomerId,
                    ProviderId = viewModel.ProviderId,
                    ServiceId = viewModel.ServiceId,
                    OrderDate = viewModel.OrderDate,
                    Status = viewModel.Status,
                    Comment = viewModel.Comment
                };

                _context.Add(request);
                await _context.SaveChangesAsync();
                TempData["Success"] = ResourceWeb.lbSuccess;
                Notification notification = new Notification
                {
                    
                    UserId = request.ProviderId,
                    Message = request.Comment, 
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };

                _context.notifications.Add(notification);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            await PopulateDropdownsAsync(viewModel);
            return View(viewModel);
        }

        /// <summary>
        /// Helper method to populate dropdown lists in the view model.
        /// </summary>
        private async Task PopulateDropdownsAsync(RequestViewModel viewModel)
        {
            viewModel.Customers = await _context.customers.ToListAsync();
            viewModel.Providers = await _context.providers.ToListAsync();
            viewModel.Services = await _context.services.ToListAsync();
        }
    }
}
