using Domin.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.Entity
{
    public  class Request
    {
       /// <summary>
       /// /////////////////////////////////////
       /// </summary>
        [Key]
        public int RequestId { get; set; }
        public int CustomerId { get; set; }
        public int ProviderId { get; set; }
        public int ServiceId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Status { get; set; }
        public string Comment { get; set; }
        [ForeignKey("CustomerId")]

        public virtual Customer Customer { get; set; }

        [ForeignKey("ServiceId")]

        public virtual Service Service { get; set; }
        [ForeignKey("ProviderId")]

        public virtual Provider Provider { get; set; }


        
    }
}
