using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Security.Claims;
using Domin.Entity;
using Infarstuructre.ViewModel;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Infarstuructre.Data;
using System.Linq;
using System.Net.NetworkInformation;

namespace MyService.Controllers
{
    public class RequestsController : Controller
    {
        private readonly MyServiceDbContext _context;
        private readonly ILogger<RequestsController> _logger;

        public RequestsController(
            MyServiceDbContext context,
            ILogger<RequestsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: عرض جميع الطلبات


        // GET: عرض نموذج إنشاء الطلب
        public IActionResult Create()
        {
            var vm = new RequestVM
            {
                AvailableServices = _context.services.ToList(),
                AvailableProviders = _context.providers.ToList()
            };
            return View(vm);
        }

        // POST: حفظ الطلب
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RequestVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // الحصول على المستخدم الحالي (افتراضيًا)
                    var currentUser = await _context.customers.FirstOrDefaultAsync(); // استبدل بمنطق المصادقة الفعلي
                    ViewBag.Providers = _context.providers.ToList(); // ✔️
                    ViewBag.Services = _context.services.ToList(); // ✔️
                    var request = new Request
                    {
                        CustomerId = currentUser.CustomerId,
                        ProviderId = vm.ProviderId,
                        ServiceId = vm.ServiceId,
                        OrderDate = DateTime.Now,
                        Status = false,
                        Comment = vm.Comment
                    };

                    _context.Add(request);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "تم إنشاء الطلب بنجاح!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "حدث خطأ أثناء حفظ البيانات: " + ex.Message);
                }
            }

            // إعادة تعبئة القوائم في حالة الخطأ
            vm.AvailableServices = _context.services.ToList();
            vm.AvailableProviders = _context.providers.ToList();
            return View(vm);
        }

        // GET: عرض جميع الطلبات
        public async Task<IActionResult> Index()
        {
            var requests = await _context.requests
                .Include(r => r.Customer)
                .Include(r => r.Provider)
                .Include(r => r.Service)
                .ToListAsync();
            ViewBag.Services = _context.services.ToList(); // ✔️
            return View(requests);
        }
    }
}