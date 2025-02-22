using Microsoft.AspNetCore.Mvc;
using Domin.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infarstuructre.Data;
using Infarstuructre.ViewModel;
using System.Linq;
using System.Threading.Tasks;
namespace MyService.Controllers
{
   

    namespace MyService.Controllers
    {
        public class RequestController : Controller
        {
            private readonly MyServiceDbContext _context;

            public RequestController(MyServiceDbContext context)
            {
                _context = context;
            }

            // GET: Request
            public async Task<IActionResult> Index()
            {
                var requests = await _context.requests
                    .Include(r => r.Customer)
                    .Include(r => r.Provider)
                    .Include(r => r.Service)
                    .ToListAsync();

                return View(requests);
            }

            // GET: Request/Create
            public IActionResult Create()
            {
                var viewModel = new RequestViewModel
                {
                    Customers = _context.customers.ToList(),
                    Providers = _context.providers.ToList(),
                    Services = _context.services.ToList()
                };

                return View(viewModel);
            }

            // POST: Request/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(RequestViewModel viewModel)
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
                    return RedirectToAction(nameof(Index));
                }

                // Reload dropdowns if validation fails
                viewModel.Customers = _context.customers.ToList();
                viewModel.Providers = _context.providers.ToList();
                viewModel.Services = _context.services.ToList();

                return View(viewModel);
            }

            // GET: Request/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var request = await _context.requests.FindAsync(id);
                if (request == null)
                {
                    return NotFound();
                }

                var viewModel = new RequestViewModel
                {
                    RequestId = request.RequestId,
                    CustomerId = request.CustomerId,
                    ProviderId = request.ProviderId,
                    ServiceId = request.ServiceId,
                    OrderDate = request.OrderDate,
                    Status = request.Status,
                    Comment = request.Comment,
                    Customers = _context.customers.ToList(),
                    Providers = _context.providers.ToList(),
                    Services = _context.services.ToList()
                };

                return View(viewModel);
            }

            // POST: Request/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, RequestViewModel viewModel)
            {
                if (id != viewModel.RequestId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var request = await _context.requests.FindAsync(id);
                        request.CustomerId = viewModel.CustomerId;
                        request.ProviderId = viewModel.ProviderId;
                        request.ServiceId = viewModel.ServiceId;
                        request.OrderDate = viewModel.OrderDate;
                        request.Status = viewModel.Status;
                        request.Comment = viewModel.Comment;

                        _context.Update(request);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RequestExists(viewModel.RequestId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }

                viewModel.Customers = _context.customers.ToList();
                viewModel.Providers = _context.providers.ToList();
                viewModel.Services = _context.services.ToList();

                return View(viewModel);
            }

            // GET: Request/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var request = await _context.requests
                    .Include(r => r.Customer)
                    .Include(r => r.Provider)
                    .Include(r => r.Service)
                    .FirstOrDefaultAsync(m => m.RequestId == id);

                if (request == null)
                {
                    return NotFound();
                }

                return View(request);
            }

            // POST: Request/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var request = await _context.requests.FindAsync(id);
                _context.requests.Remove(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool RequestExists(int id)
            {
                return _context.requests.Any(e => e.RequestId == id);
            }
        }
    }
}
