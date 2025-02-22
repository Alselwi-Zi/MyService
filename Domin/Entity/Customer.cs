using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domin.Entity
{
    public  class Customer
    {
        /// <summary>
        /// /////////
        /// </summary>

        [Key]
        public int CustomerId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<Request>? Requests { get; set; }
    }
}
