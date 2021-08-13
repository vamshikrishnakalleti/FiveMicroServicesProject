using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Database
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }

        [NotMapped]
        public string[] Roles { get; set; }
    }
}
