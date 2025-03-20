using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using HospitalManagementSystemDAL;
using HospitalManagementSystemDAL.Models;
using HospitalManagementSystemShared;
using HospitalManagementSystemShared.Constants;
using HospitalManagementSystemShared.Utilities;
using HospitalManagementSystemShared.ViewModels;


namespace HospitalManagementSystemBAL.Services
{
    public class AppointmentService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly DoctorService _doctorService;
        private readonly Email _email;

        public AppointmentService()
        {
            _unitOfWork = new UnitOfWork();
            AutoMapperConfig.RegisterMappings();
            _doctorService = new DoctorService();
            _email = new Email();
        }

        private static void GetDataTableFilters(DataTableVM model, out string searchValue, out string sortColumnName,
            out string sortDirection, out int start, out int length)
        {
             searchValue = model.Search?.Value ?? string.Empty;
             start = model.Start;
             length = model.Length;
             int sortColumnIndex = (model.Order != null && model.Order.Count > 0) ? model.Order[0].Column : 0;

             sortColumnName = (model.Columns != null && model.Columns.Count > sortColumnIndex)
                ? model.Columns[sortColumnIndex]?.Name
                : "Id";

            sortDirection = (model.Order != null && model.Order.Count > 0)
                          ? model.Order[0].Dir : "";
        }

        //Create
        public async Task CreateAppointment(AppointmentVM appointmentVM)
        {
            var appointment = Mapper.Map<AppointmentVM, Appointment>(appointmentVM);
           
            //Email sending
            string doctorId = appointment?.DoctorId;
            string doctorEmail =_doctorService.GetDoctorById(doctorId)?.Email;
            string doctorName = _doctorService.GetDoctorById(doctorId)?.FullName;
            string patientName  = appointment?.PatientName;
            string consultationDate = appointment?.ConsultationDate?.ToLongDateString();

            string htmlBody = EmailTemplates.DoctorConfirmEmailTemplate(doctorName, patientName,consultationDate);

            await _unitOfWork.AppointmentRepo.CreateAppointment(appointment);

            if (!string.IsNullOrEmpty(doctorEmail) && !string.IsNullOrEmpty(doctorName))
            {
                _email.SendEmail(doctorEmail, htmlBody, EmailSubjectsConstants.AppointmentConfirm);
            }
        }


        public AppointmentVM GetAppointmentById(int id)
        {
            var appointment = _unitOfWork.AppointmentRepo.GetAppointmentById(id);                       
           return Mapper.Map<Appointment, AppointmentVM>(appointment);
        }

        public AppointmentViewModel GetAppointmentViewModel(AppointmentVM appointment)
        {
            var status = StatusList.GetStatusList();

            var items = status.Select(s => new SelectListItem
                        {
                           Text = s,
                           Value = s,
                           Selected = (s == appointment.AppointmentStatus)
                        }).ToList();

            var appointmentVM = new AppointmentViewModel
            {
                AppointmentVM = appointment,
                StatusList = items,
            };

            return appointmentVM;
        }

        public object GetAppointmentList(DataTableVM model)
        {
            int start, length;
            string searchValue, sortColumnName, sortDirection;

            GetDataTableFilters(model, out searchValue, out sortColumnName, out sortDirection, out start, out length);

            var appointments = _unitOfWork.AppointmentRepo.GetAllAppointments();
            int total = appointments.Count();

            //SEARCHING
            if (!string.IsNullOrEmpty(searchValue))
            {
                appointments = appointments.Where(a => a.Id.ToString().Contains(searchValue.ToLower()) ||
                                     a.PatientName.ToLower().Contains(searchValue.ToLower()) ||
                                     a.PatientEmail.ToLower().Contains(searchValue.ToLower()) ||
                                     a.User.FullName.ToLower().Contains(searchValue.ToLower()));
                                    
            }

            int filteredRecords = appointments.Count();

            //Sorting
            appointments = appointments
                          .OrderBy(sortColumnName + " " + sortDirection);

            //pagination
            var data = appointments.Skip(start).Take(length)
                       .Select(a=>
                                     new AppointmentVM
                                     {
                                         Id = a.Id,
                                         PatientName = a.PatientName,
                                         PatientEmail = a.PatientEmail,
                                         DoctorId = a.DoctorId,
                                         DoctorName = a.User.FullName,
                                         Symptoms = a.Symptoms,
                                         ConsultationDate = a.ConsultationDate,
                                         BookingDate = a.BookingDate,
                                         AppointmentStatus = a.AppointmentStatus,
                                         Priority = a.Priority,

                                     })
                       .ToList();

            return new { draw = model.Draw, data, recordsTotal = total, recordsFiltered = filteredRecords};    
        }


        public object GetAppointmentListByDoctorId(DataTableVM model, string doctorId)
        {
            int start,length;
            string searchValue, sortColumnName, sortDirection;

            GetDataTableFilters(model, out searchValue, out sortColumnName, out sortDirection, out start, out length);

            var appointments = _unitOfWork.AppointmentRepo.GetAppointmentsByDoctorId(doctorId);
            int total = appointments.Count();

            //SEARCHING
            if (!string.IsNullOrEmpty(searchValue))
            {
                appointments = appointments.Where(a => a.Id.ToString().Contains(searchValue.ToLower()) ||
                                     a.PatientName.ToLower().Contains(searchValue.ToLower()) ||
                                     a.PatientEmail.ToLower().Contains(searchValue.ToLower()));
                                   
            }

            int filteredRecords = appointments.Count();

            //Sorting
            appointments = appointments
                .OrderBy(sortColumnName + " " + sortDirection);

            //pagination
            var data = appointments.Skip(start).Take(length).AsEnumerable()
                      .Select( a=> Mapper.Map<Appointment, AppointmentVM>(a))
                      .ToList();

            return new { draw = model.Draw, data, recordsTotal = total, recordsFiltered = filteredRecords };
        }


        public AppointmentHistoryResultVM GetCompletedAppointments(string doctorId, AppointmentHistoryPostModel model)
        {
            var appointments = _unitOfWork.AppointmentRepo.GetAppointmentsByDoctorId(doctorId);

            string searchValue = model.SearchValue ?? string.Empty;
            string sortColumnName = string.IsNullOrEmpty(model.SortColumn) ? "ConsultationDate" : model.SortColumn;
            string sortDirection = string.IsNullOrEmpty(model.SortDirection) ? "desc" : model.SortDirection;         
            int page = model.Page;
            int pageSize = model.PageSize;
            int skip = (page - 1)*pageSize;

            appointments = appointments
                          .Where(a => a.AppointmentStatus == AppointmentsStatus.Completed);
                                    
            int totalRecords = appointments.Count();
            int totalPages = (int)Math.Ceiling(((double)totalRecords / (double)pageSize));

            //Searching
            if (!string.IsNullOrEmpty(searchValue))
            {
                appointments = appointments.Where(a => a.Id.ToString().Contains(searchValue.ToLower()) ||
                                     a.PatientName.ToLower().Contains(searchValue.ToLower()) ||
                                     a.PatientEmail.ToLower().Contains(searchValue.ToLower()));
            }
          
           //Sorting
           appointments = appointments
                          .OrderBy(sortColumnName + " " + sortDirection);

           //Pagination
           var data = appointments.Skip(skip).Take(pageSize).AsEnumerable()
                      .Select(a => Mapper.Map<Appointment, AppointmentVM>(a))
                      .ToList();

           var result = new AppointmentHistoryResultVM() 
                        { 
                          AppointmentVMs = data, 
                          TotalPages = totalPages 
                        };

            return result;
        }


        //Delete
        public ResponseWrapper DeleteAppointment(int id)
        {
            var appointment = _unitOfWork.AppointmentRepo.GetAppointmentById(id);

            if (appointment == null)
            {
                return new ResponseWrapper { Success = false, Message = NotificationMessages.NotFound };               
            }
            else if (appointment.AppointmentStatus == AppointmentsStatus.Confirmed || appointment.AppointmentStatus == AppointmentsStatus.Completed)
            {
                return new ResponseWrapper { Success = false, Message = NotificationMessages.CannotChangeAppointment.Replace("change", "Delete") };              
            }
            else
            {
                if (appointment.IsDeleted == false)
                {
                    appointment.IsDeleted = true;
                    appointment.AppointmentStatus = AppointmentsStatus.Cancelled;
                    _unitOfWork.Save();
                    return new ResponseWrapper { Success = true, Message = NotificationMessages.DeletionSuccess };
                }
                else
                {
                    return new ResponseWrapper { Success = false, Message = NotificationMessages.NotFound };
                }                
            }
        }

        //EDIT
        public void UpdateAppointment(AppointmentVM appointmentVM)
        {
            var appointment = Mapper.Map<AppointmentVM, Appointment>(appointmentVM);

            int appointmentId = appointment.Id;
            var oldAppointment = _unitOfWork.AppointmentRepo.GetAppointmentById(appointmentId);
            string oldDoctor = oldAppointment?.DoctorId;
            string oldDoctorEmail = _doctorService.GetDoctorById(oldDoctor)?.Email;
            string oldDoctorName = _doctorService.GetDoctorById(oldDoctor)?.FullName;
            string oldAppointmentDate = oldAppointment?.ConsultationDate?.ToLongDateString();

            if (oldDoctor != appointment.DoctorId)
            {
                string newDoctorId = appointment?.DoctorId;      
                string newDoctorEmail = _doctorService.GetDoctorById(newDoctorId)?.Email;
                string newDoctorName = _doctorService.GetDoctorById(newDoctorId)?.FullName;
                string patientName = appointment?.PatientName;
                string consultationDate = appointment?.ConsultationDate?.ToLongDateString();

               _unitOfWork.AppointmentRepo.UpdateAppointment(appointment);

                if(!string.IsNullOrEmpty(oldDoctorEmail) && !string.IsNullOrEmpty(oldDoctorName))
                {
                    string cancelEmailBody = EmailTemplates.DoctorCancelEmailTemplate(oldDoctorName, oldAppointmentDate);
                    _email.SendEmail(oldDoctorEmail, cancelEmailBody, EmailSubjectsConstants.AppointmentCancel);
                }
              
                if(!string.IsNullOrEmpty(newDoctorEmail) && !string.IsNullOrEmpty(newDoctorName))
                {
                    string htmlBody = EmailTemplates.DoctorConfirmEmailTemplate(newDoctorName, patientName, consultationDate);
                    _email.SendEmail(newDoctorEmail, htmlBody, EmailSubjectsConstants.AppointmentConfirm);
                }               
            }
            else
            {
                _unitOfWork.AppointmentRepo.UpdateAppointment(appointment);
            }
        }

    }
}
