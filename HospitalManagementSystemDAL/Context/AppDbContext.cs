using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using HospitalManagementSystemDAL.Models;
using System.Configuration;

namespace HospitalManagementSystemDAL.Context
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public virtual List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }


    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["AppDbContext"].ToString();

        public AppDbContext() : base(_connectionString, throwIfV1Schema: false)
        {
        }
        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        public DbSet<Appointment> Appointments { get; set;}
       
    }
}
