using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Entity
{
    public class Service
    {
        [Key]

        public int ServiceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }

        public bool IsActive { get; set; }
        public virtual ICollection<Request> Requests { get; set; }

    }
}
