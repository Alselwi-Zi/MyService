using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domin.Entity;
using System.ComponentModel.DataAnnotations;
namespace Infarstuructre.ViewModel
{
    

   
        public class RequestViewModel
        {
            public int RequestId { get; set; }

            [Required(ErrorMessage = "العميل مطلوب")]
            [Display(Name = "العميل")]
            public int CustomerId { get; set; }

            [Required(ErrorMessage = "المزود مطلوب")]
            [Display(Name = "المزود")]
            public int ProviderId { get; set; }

            [Required(ErrorMessage = "الخدمة مطلوبة")]
            [Display(Name = "الخدمة")]
            public int ServiceId { get; set; }

            [Required(ErrorMessage = "تاريخ الطلب مطلوب")]
            [Display(Name = "تاريخ الطلب")]
            public DateTime OrderDate { get; set; } = DateTime.Now;

            [Display(Name = "الحالة")]
            public bool Status { get; set; }

            [Display(Name = "ملاحظات")]
            public string Comment { get; set; }

            // Dropdown lists
            public List<Customer> Customers { get; set; }
            public List<Provider> Providers { get; set; }
            public List<Service> Services { get; set; }
        }
    }

