using Domin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.ViewModel
{
    public class ServiceViewModel
    {
        public NewService NewService { get; set; }
        public List<Service> services { get; set; } 
    }
    public class NewService
    {
        public int ServiceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
