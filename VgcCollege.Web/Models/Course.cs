using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Web.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
}