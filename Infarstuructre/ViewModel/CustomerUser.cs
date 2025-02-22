using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.ViewModel
{

 
    public class CustomerUser : IdentityUser
        {


        
            public string Name { get; set; }
            public bool ActiveUser { get; set; }
        

    }
    }

