using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Web.Models
{
    public class ExamResult
    {
        public int Id { get; set; }

        
        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; }

       
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        
        [Range(0, 100)]
        public double Grade { get; set; }
    }
}