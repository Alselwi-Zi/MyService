using Domin.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.ViewModel
{


   
        public class RequestVM
        {
            [Required(ErrorMessage = "يجب اختيار الخدمة")]
            [Display(Name = "الخدمة المطلوبة")]
            public int ServiceId { get; set; }

            [Required(ErrorMessage = "يجب اختيار المزود")]
            [Display(Name = "المزود")]
            public int ProviderId { get; set; }

            [Display(Name = "ملاحظات إضافية")]
            [StringLength(500, ErrorMessage = "الملاحظات لا يجب أن تتجاوز 500 حرف")]
            public string Comment { get; set; }

            // قوائم الاختيار
            public List<Service> AvailableServices { get; set; }
            public List<Provider> AvailableProviders { get; set; }
        }
    
}