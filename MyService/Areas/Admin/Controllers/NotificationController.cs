using Domin.Entity;
using Infarstuructre.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MyService.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NotificationController : Controller
    {
        private readonly MyServiceDbContext _context;

        public NotificationController(MyServiceDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Notification
        public async Task<IActionResult> Index()
        {
            // Retrieve all notifications, including customer details, ordered by newest first.
            var notifications = await _context.notifications
                .Include(n => n.Customer)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return View(notifications);
        }

        // GET: Admin/Notification/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.notifications
                .Include(n => n.Customer)
                .FirstOrDefaultAsync(n => n.NotificationId == id);
            if (notification == null)
            {
                return NotFound();
            }

            // Optionally mark the notification as read when viewing details.
            if (!notification.IsRead)
            {
                notification.IsRead = true;
                _context.Update(notification);
                await _context.SaveChangesAsync();
            }

            return View(notification);
        }

        // POST: Admin/Notification/MarkAsRead/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var notification = await _context.notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            notification.IsRead = true;
            _context.Update(notification);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
