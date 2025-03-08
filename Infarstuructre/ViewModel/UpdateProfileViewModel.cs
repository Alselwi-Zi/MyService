using Domin.Resource;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.ViewModel
{
    // نموذج الفيو المحدث
    public class UpdateProfileViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string CurrentImage { get; set; } // الصورة الحالية

        [DataType(DataType.Upload)]
        
        public string ProfileImage { get; set; } // ملف الصورة الجديد

        public PasswordUpdateViewModel PasswordUpdate { get; set; }
    }

    internal class AllowedExtensionsAttribute : Attribute
    {
    }

    public class PasswordUpdateViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور الحالية")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6)]
        [Display(Name = "كلمة المرور الجديدة")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Display(Name = "تأكيد كلمة المرور")]
        public string ComparePassword { get; set; }
    }
}

