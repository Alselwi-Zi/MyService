using Domin.Resource;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.ViewModel
{
    public class UpdateProfileViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string cImage { get; set; }
        public ChangePasswordViewModel ChangePassword { get; set; }

    }
}

