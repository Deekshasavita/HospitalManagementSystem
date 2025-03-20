using System.Linq;
using System.Threading.Tasks;
using HospitalManagementSystemDAL.Models;

namespace HospitalManagementSystemDAL.Repositories
{
    public interface IAppointmentRepository
    {
        IQueryable<Appointment> GetAllAppointments();
        Appointment GetAppointmentById(int id);
        void UpdateAppointment(Appointment appointment);
        Task CreateAppointment(Appointment appointment);
        IQueryable<Appointment> GetAppointmentsByDoctorId(string doctorId);
    }
}
