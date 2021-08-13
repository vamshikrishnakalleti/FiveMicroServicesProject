using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Database
{
    public class DatabaseContext: IdentityDbContext<User, Role, int>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
        {

        }
    }
}
