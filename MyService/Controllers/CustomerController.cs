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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace MyService.Controllers
{
    [Authorize]

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


        [HttpGet]
        public async Task<IActionResult> UpdateProfile(string id)
        {
            // التحقق من مطابقة المستخدم
            if (string.IsNullOrEmpty(id) || id != _userManager.GetUserId(User))
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UpdateProfileViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                CurrentImage = user.ImageUser // تخزين الصورة الحالية
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null || user.Id != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            // التحقق من البريد الإلكتروني الجديد في حال تغييره
            if (user.Email != model.Email)
            {
                var emailExists = await _userManager.FindByEmailAsync(model.Email);
                if (emailExists != null)
                {
                    ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل");
                    return View(model);
                }
            }

            // تحديث الصورة في حال تم رفع ملف
            if (HttpContext.Request.Form.Files.Count > 0)
            {
                var file = HttpContext.Request.Form.Files[0];
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine("wwwroot", Helper.PathSaveImageuser, imageName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                // تحديث الخاصية الخاصة بصورة المستخدم
                user.ImageUser = imageName;
                model.CurrentImage = imageName;
            }
            // وإلا يتم الاحتفاظ بالصورة الحالية دون تغيير

            // تحديث البيانات الأساسية
            user.Name = model.Name;
            user.Email = model.Email;
            user.UserName = model.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                AddErrorsToModelState(updateResult.Errors);
                return View(model);
            }

            // تحديث كلمة المرور إن كان المستخدم يريد تغييرها
            if (!string.IsNullOrEmpty(model.PasswordUpdate?.NewPassword))
            {
                if (model.PasswordUpdate.NewPassword != model.PasswordUpdate.ComparePassword)
                {
                    ModelState.AddModelError("PasswordUpdate.ComparePassword", "كلمات المرور غير متطابقة");
                    return View(model);
                }

                // يجب استخدام الحقل المخصص لكلمة المرور الحالية لتغيير كلمة المرور
                var changeResult = await _userManager.ChangePasswordAsync(
                    user,
                    model.PasswordUpdate.CurrentPassword, // استخدام كلمة المرور الحالية
                    model.PasswordUpdate.NewPassword
                );

                if (!changeResult.Succeeded)
                {
                    AddErrorsToModelState(changeResult.Errors);
                    return View(model);
                }
            }

            TempData["SuccessMessage"] = "تم تحديث الملف الشخصي بنجاح";
            return RedirectToAction("Index", "Home");
        }

        // دالة مساعدة لإضافة الأخطاء
        private void AddErrorsToModelState(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // نموذج الفيو المحدث
        
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ChangePassword(RegisterViewModel model)
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