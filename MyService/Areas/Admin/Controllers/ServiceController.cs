using Domin.Entity;
using MyService.Resource;
using Infarstuructre.Data;
using Infarstuructre.ViewModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyService.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly MyServiceDbContext _context;
        public ServiceController(MyServiceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Service()
        {
            var model = new ServiceViewModel
            {
                NewService = new NewService(),
                services = await _context.services.Include(x => x.Requests).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Service(ServiceViewModel service)
        {
            
                try
                {
                    var serviceInput = new Service()
                    {
                        ServiceID = service.NewService.ServiceID,
                        Name = service.NewService.Name,
                        Description = service.NewService.Description,
                        CreatedAt = service.NewService.CreatedAt,
                        IsActive = service.NewService.IsActive
                    };

                    if (serviceInput.ServiceID == 0)
                    {
                        await _context.services.AddAsync(serviceInput);
                        await _context.SaveChangesAsync();
                        TempData["Success"] = ResourceWeb.lbSuccess;
                    }
                    else
                    {
                        var serviceUpdate = await _context.services.FindAsync(serviceInput.ServiceID);
                        if (serviceUpdate != null)
                        {
                            serviceUpdate.Name = serviceInput.Name;
                            serviceUpdate.Description = serviceInput.Description;
                            serviceUpdate.CreatedAt = serviceInput.CreatedAt;
                            serviceUpdate.IsActive = serviceInput.IsActive;
                            _context.services.Update(serviceUpdate);
                            await _context.SaveChangesAsync();
                            TempData["Update"] = ResourceWeb.lbUpdate;
                        }
                        else
                        {
                            TempData["Error"] = "Service not found.";
                        }
                    }
                }
                catch (Exception)
                {
                    TempData["Error"] = "An error occurred while saving the service.";
                    return RedirectToAction("Service");
                }
            
            return RedirectToAction("Service");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteService(int id)
        {
            var model = await _context.services.FindAsync(id);
            try
            {
                if (model != null)
                {
                    _context.services.Remove(model);
                    await _context.SaveChangesAsync();
                    TempData["Delete"] = ResourceWeb.lbTitleDeletedOk;
                }
                else
                {
                    TempData["Error"] = "Service not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An unexpected error occurred: {ex.Message}";
            }

            return RedirectToAction("Service");
        }
    }
}