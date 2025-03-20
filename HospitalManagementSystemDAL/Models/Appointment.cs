using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HospitalManagementSystemDAL.Context;
using HospitalManagementSystemShared.Constants;

namespace HospitalManagementSystemDAL.Models
{
    public class Appointment
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(RegExPatterns.NameRegEx)]
        public string PatientName { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(RegExPatterns.EmailRegEx)]
        public string PatientEmail { get; set; }

        [Required]
        public string DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual ApplicationUser User { get; set; }


        [StringLength(200)]
        public string Symptoms { get; set; }
        public string AppointmentStatus { get; set; } = AppointmentsStatus.Pending;

        [Required]
        public DateTime? ConsultationDate { get; set; }
        public DateTime BookingDate { get;set; } = DateTime.Now;
        public string Priority { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
