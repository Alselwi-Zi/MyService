using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domin.Entity
{
    public  class Provider
    {


        [Key]
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Address { get; set; }

        public virtual ICollection<Request>? Requests { get; set; }
    }
}
