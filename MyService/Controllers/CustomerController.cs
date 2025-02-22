using Domin.Entity;
using Infarstuructre.Data;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace MyService.Controllers
{
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly MyServiceDbContext _context;
        public CustomerController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, MyServiceDbContext context)

        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Customer/Register
        [AllowAnonymous]
        public IActionResult Registers()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Registers(RegisterCustomerViewModel model)
        {
            // Retrieve the existing user by email.
            var existingUser = await _userManager.FindByEmailAsync(model.RegisterCustomer.Email);

            // Check if a user with the given email exists.
            if (existingUser != null)
            {
                // The email already exists. Add a model error and return the view.
                ModelState.AddModelError("RegisterCustomer.Email", "This email is already registered. Please use another email address.");
                return View(model);
            }

            IFormFile file = null;
            var user = new ApplicationUser
            {
                Id = model.RegisterCustomer.Id,
                Name = model.RegisterCustomer.Name,
                UserName = model.RegisterCustomer.Email,
                Email = model.RegisterCustomer.Email,
                ActiveUser = model.RegisterCustomer.ActiveUser,
                ImageUser = await HandleImageUpload(file) // دالة معالجة الصورة



            };
                if (user.Id == null)
                {
                    //Craete
                    user.Id = Guid.NewGuid().ToString();
                    var result = await _userManager.CreateAsync(user, model.RegisterCustomer.Password);
                    if (result.Succeeded)
                    {
                        //Succsseded
                        var Role = await _userManager.AddToRoleAsync(user, Helper.Roles.Basic.ToString());
                        if (Role.Succeeded)
                            SessionMsg(Helper.Success, Resource.ResourceWeb.lbSave, Resource.ResourceWeb.lbNotSavedMsgUserRole);
                        else
                            SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbNotSavedMsgUser);
                    }
                    else //Not Successeded
                        SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbNotUpdateMsgUser);
                }
                
                return RedirectToAction("Login", "Customer");
            
        }


        // GET: Customer/Login


        // GET: Account/EditProfile
        public async Task<IActionResult> UpdateProfile(string id)
        {
            // Ensure the user is accessing their own profile.
            if (string.IsNullOrEmpty(id) || id != _userManager.GetUserId(User))
            {
                return RedirectToAction("Login");
            }

            // Retrieve the user from the database.
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Create a view model and pre-fill with user data.
            var model = new UpdateProfileViewModel
            {
                Email = user.Email,
                Name = user.Name,
                cImage = user.ImageUser,
                // The ChangePassword object can be initialized here if needed.
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            // Check if the submitted model is valid.
            if (!ModelState.IsValid)
            {
                // Aggregate errors and pass via TempData.
                var errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["ErrorMessage"] = errors;
                return View(model);
            }

            // Retrieve the current user's ID.
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User is not logged in.";
                return RedirectToAction("Login", "Customer");
            }

            // Retrieve the user from the database.
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return NotFound();
            }

            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Helper.PathSaveImageuser, imageName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file[0].CopyToAsync(fileStream);
                }
                user.ImageUser = imageName;
            }
            user.UserName = model.Name;
            user.Email = model.Email;
            if (user is ApplicationUser appUser)
            {
                appUser.Name = model.Name;
            }

            // Save updates to the database.
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = string.Join(" ", result.Errors.Select(e => e.Description));
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ChangePassword(UpdateProfileViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.ChangePassword.Id);
            if (user != null)
            {
                await _userManager.RemovePasswordAsync(user);
                var AddNewPassword = await _userManager.AddPasswordAsync(user, model.ChangePassword.NewPassword);
                if (AddNewPassword.Succeeded)
                    SessionMsg(Helper.Success, Resource.ResourceWeb.lbSave, Resource.ResourceWeb.lbMsgSavedChangePassword);
                else
                    SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgNotSavedChangePassword);

                return RedirectToAction(nameof(Registers));
            }

            return RedirectToAction(nameof(Registers));

        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Result = await _signInManager.PasswordSignInAsync(model.Eamil,
                    model.Password, model.RememberMy, false);
                if (Result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    ViewBag.ErrorLogin = false;
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Customer");
        }
        private async Task<string> HandleImageUpload(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return "wwwroot/img/testimonial-3.jpg"; // مسار الصورة الافتراضية
            }

            // التأكد من أن الملف صورة
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                throw new Exception("Invalid file type. Allowed types: JPG, JPEG, PNG, GIF");
            }
            // إنشاء مجلد إذا لم يكن موجوداً
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Customers");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // إنشاء اسم فريد للملف
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // حفظ الملف
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/Images/Customers/{uniqueFileName}";
        }
        
        private void SessionMsg(string MsgType, string Title, string Msg)
        {
            HttpContext.Session.SetString(Helper.MsgType, MsgType);
            HttpContext.Session.SetString(Helper.Title, Title);
            HttpContext.Session.SetString(Helper.Msg, Msg);
        }
    }
    }