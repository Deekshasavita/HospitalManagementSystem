using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HospitalManagementSystemDAL;
using HospitalManagementSystemDAL.Context;
using HospitalManagementSystemDAL.Models;
using HospitalManagementSystemShared.Utilities;
using HospitalManagementSystemShared.ViewModels;

namespace HospitalManagementSystemBAL.Services
{
    public class DoctorService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly Email _email;

        public DoctorService()
        {
            _unitOfWork = new UnitOfWork();
            _email = new Email();   
            AutoMapperConfig.RegisterMappings();
        }

        public List<DoctorVM> GetDoctors()
        {
            var doctors = _unitOfWork.DoctorRepo.GetAllDoctors()
                         .Select(d => Mapper.Map<ApplicationUser, DoctorVM>(d)).ToList();
            return doctors;

        }

        public List<DoctorVM> GetAllAvailableDoctors()
        {
            var doctors = _unitOfWork.DoctorRepo.GetAllAvailableDoctors().AsEnumerable()
                         .Select(dr=>Mapper.Map<ApplicationUser,DoctorVM>(dr)).ToList();
            return doctors;
        }

        public DoctorVM GetDoctorById(string id)
        {
            return Mapper.Map<ApplicationUser, DoctorVM>(_unitOfWork.DoctorRepo.GetDoctorById(id));
        }

        public void UpdateAppointmentStatus(AppointmentVM appointmentVM)
        {
            var appointment = Mapper.Map<AppointmentVM,Appointment>(appointmentVM);

            //Email
            string patient = appointment?.PatientName;
            string patientEmail = appointment?.PatientEmail;
            string date = appointment?.ConsultationDate?.ToLongDateString();
            string doctorId = appointment?.DoctorId;
            string doctor = _unitOfWork.DoctorRepo.GetDoctorById(doctorId)?.FullName;

            string htmlBody = EmailTemplates.PatientEmailTemplate(doctor, patient, date);

            _unitOfWork.AppointmentRepo.UpdateAppointment(appointment);

            if(!string.IsNullOrEmpty(patientEmail) && !string.IsNullOrEmpty(patient))
            {
                _email.SendEmail(patientEmail, htmlBody, EmailSubjectsConstants.AppointmentConfirm);
            }           
        }
    }
}
