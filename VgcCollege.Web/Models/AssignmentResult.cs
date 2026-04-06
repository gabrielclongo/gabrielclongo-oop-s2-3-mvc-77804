using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Web.Models
{
    public class AssignmentResult
    {
        public int Id { get; set; }

        public int AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }

        public int StudentProfileId { get; set; }
        public StudentProfile? StudentProfile { get; set; }

        [Range(0, 100)]
    
        public double Grade { get; set; }
    }
}