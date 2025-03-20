using System.Collections.Generic;
using System.Linq;
using HospitalManagementSystemDAL.Context;

namespace HospitalManagementSystemDAL.Repositories
{
    public interface IDoctorRepository
    {
        IQueryable<ApplicationUser> GetAllDoctors();
        IQueryable<ApplicationUser> GetAllAvailableDoctors();
        ApplicationUser GetDoctorById(string id);
    }
}
