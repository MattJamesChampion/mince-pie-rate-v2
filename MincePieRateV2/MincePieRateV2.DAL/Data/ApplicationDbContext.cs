using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MincePieRateV2.Web.Models.Domain;

namespace MincePieRateV2.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<MincePie> MincePies { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
