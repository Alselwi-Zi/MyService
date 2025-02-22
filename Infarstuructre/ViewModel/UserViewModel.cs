using Domin.Entity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
namespace Infarstuructre.ViewModel
{
    public class UserViewModel:IdentityUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }

}