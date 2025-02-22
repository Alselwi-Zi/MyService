using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Entity
{
    public class Employees
    {[Key]
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = ("Name"))]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MaxLength")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MinLength")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = ("RegisterEmail"))]

        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = ("PhoneNamber"))]

        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } 
        
        public string TitleResume { get; set; }
        public int CurrentStatus { get; set; }
        public DateTime? Created { get; set; }=DateTime.Now;
        public List<EmployeeRequest> employeeRequests{ get; set; }

    }
}
