using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kintai.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "勤務日")]
        public DateTime WorkDate { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "出勤")]
        public DateTime? ClockIn { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "退勤")]
        public DateTime? ClockOut { get; set; }

        [Display(Name = "勤務時間")]
        public TimeSpan WorkingHours { get; set; }

        [Display(Name = "社員")]
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
