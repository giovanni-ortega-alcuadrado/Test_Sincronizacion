using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace A2OYD_Servicios_API.ContextosDB
{
    public class ContextoDbUtil : IdentityDbContext<A2OYD_Servicios_API.Models.Seguridad.ApplicationUser>
    {
        public ContextoDbUtil(DbContextOptions<ContextoDbUtil> opciones) : base(opciones)
        {
        }
    }
}
