using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.Entity
{
    public  class EmployeeRequest
    {
        public int EmployeeRequestId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? CreatedAt { get; set; }
        [ForeignKey("EmployeeId")]
        public  Employees? Employee { get; set; }
    }
}
