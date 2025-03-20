using HospitalManagementSystemDAL.Context;
using HospitalManagementSystemDAL.Repositories;

namespace HospitalManagementSystemDAL
{
    public class UnitOfWork
    {
        private IAppointmentRepository _appointmentRepository;
        private IDoctorRepository _doctorRepository;
        private readonly AppDbContext _appDbContext;

        public UnitOfWork()
        {
            _appDbContext = new AppDbContext();
        }

        public IAppointmentRepository AppointmentRepo
        {
            get
            {
                if (_appointmentRepository == null)
                {
                    _appointmentRepository = new AppointmentRepository(_appDbContext);
                }
                return _appointmentRepository;
            }
        }

        public IDoctorRepository DoctorRepo
        {
            get
            {
                if (_doctorRepository == null)
                {
                    _doctorRepository = new DoctorRepository(_appDbContext);
                }
              return _doctorRepository;
            }
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
