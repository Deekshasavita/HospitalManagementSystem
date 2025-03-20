using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HospitalManagementSystemDAL.Context;
using HospitalManagementSystemDAL.Models;

namespace HospitalManagementSystemDAL.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;
        public AppointmentRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        //Create
        public async Task CreateAppointment(Appointment appointment)
        {
            if (appointment != null)
            {
                _context.Appointments.Add(appointment);
                 await _context.SaveChangesAsync();
            }
        }

        public IQueryable<Appointment> GetAllAppointments()
        {
            return _context.Appointments.Include(a=>a.User)
                .Where(a => a.IsDeleted == false);
        }

        public IQueryable<Appointment> GetAppointmentsByDoctorId(string doctorId)
        {
            return _context.Appointments.Include(a=>a.User)
                .Where(a =>a.DoctorId == doctorId && a.IsDeleted == false);
        }

        public Appointment GetAppointmentById(int id)
        {
            return _context.Appointments.Include(a => a.User)
             .Where(a => a.IsDeleted == false && a.Id == id).FirstOrDefault();                 
        }

        //Edit
        public void UpdateAppointment(Appointment appointment)
        {
            if (appointment != null) 
            {
                Appointment oldAppointment = _context.Appointments.Find(appointment.Id);

                _context.Entry(oldAppointment).State = EntityState.Detached;
                _context.Entry(appointment).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
