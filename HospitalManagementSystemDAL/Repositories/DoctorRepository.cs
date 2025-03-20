using System.Data.Entity;
using System.Linq;
using HospitalManagementSystemDAL.Context;
using HospitalManagementSystemShared.Constants;

namespace HospitalManagementSystemDAL.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly string _doctorRoleId;

        public DoctorRepository(AppDbContext appDbContext)
        {
            _doctorRoleId = UserConstants.DoctorRoleId;
            _appDbContext = appDbContext;        
        }

        public IQueryable<ApplicationUser> GetAllDoctors()
        {
            var allDoctors = _appDbContext.Users
                             .Include(u => u.Roles)
                             .Where(u => u.Roles.Any(ur => ur.RoleId == _doctorRoleId));
            return allDoctors;
        }

        public  IQueryable<ApplicationUser> GetAllAvailableDoctors()
        {
          var allAvailableDoctors =  _appDbContext.Users
                                    .Include(u => u.Roles)
                                    .Where(u => u.Roles.Any(ur => ur.RoleId == _doctorRoleId) &&
                                    u.Appointments
                                    .Count(a => a.AppointmentStatus == AppointmentsStatus.Pending || 
                                    a.AppointmentStatus == AppointmentsStatus.Confirmed) < 5);
                   
           return allAvailableDoctors;
        }

        public ApplicationUser GetDoctorById(string id)
        {
            return _appDbContext.Users.Find(id);
        }
    }
}
