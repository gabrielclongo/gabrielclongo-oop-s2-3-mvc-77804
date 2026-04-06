using System.ComponentModel.DataAnnotations;

namespace VgcCollege.Web.Models
{
    public class StudentProfile
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string IdentityUserId { get; set; } = string.Empty;
    }
}