using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Web.Models
{
    public class CourseEnrollment
    {
        public int Id { get; set; }

        public int StudentProfileId { get; set; }
        public StudentProfile? StudentProfile { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public DateTime EnrolDate { get; set; }
        public string? Status { get; set; }
    }
}