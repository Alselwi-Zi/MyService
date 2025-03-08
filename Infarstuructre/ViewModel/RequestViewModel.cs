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

            [Required]
            [Display(Name = "Customer")]
            public int CustomerId { get; set; }

            [Required]
            [Display(Name = "Provider")]
            public int ProviderId { get; set; }

            [Required]
            [Display(Name = "Service")]
            public int ServiceId { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Order Date")]
            public DateTime OrderDate { get; set; }

            [Required]
            public bool Status { get; set; }

            public string Comment { get; set; }

            // Collections to populate dropdowns in the view
            public IEnumerable<Customer> Customers { get; set; }
            public IEnumerable<Provider> Providers { get; set; }
            public IEnumerable<Service> Services { get; set; }
        }
    }


