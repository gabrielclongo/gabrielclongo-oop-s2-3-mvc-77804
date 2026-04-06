using System;
using System.Collections.Generic;

namespace VgcCollege.Web.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

       
        public ICollection<CourseEnrollment>? Enrollments { get; set; }
        public ICollection<Assignment>? Assignments { get; set; }
        public ICollection<Exam>? Exams { get; set; }
    }
}