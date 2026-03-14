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
        public DateTime WorkDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime? ClockIn { get; set; }

        [DataType(DataType.Time)]
        public DateTime? ClockOut { get; set; }

        public TimeSpan WorkingHours { get; set; }


        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
