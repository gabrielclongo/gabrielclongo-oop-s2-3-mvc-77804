namespace VgcCollege.Web.Models
{
    public class AttendanceRecord
    {
        public int Id { get; set; }

        public int CourseEnrollmentId { get; set; }
        public CourseEnrollment? CourseEnrollment { get; set; }

        public int WeekNumber { get; set; }

        public bool Present { get; set; }
    }
}