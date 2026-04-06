using System.Collections.Generic;

namespace VgcCollege.Web.Models
{
    public class GradebookViewModel
    {
        public List<AssignmentResult> AssignmentResults { get; set; }
        public List<ExamResult> ExamResults { get; set; }
    }
}