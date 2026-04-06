using System;
using Microsoft.AspNetCore.Identity;

namespace VgcCollege.Web.Models
{
    public class StudentProfile
    {
        public int Id { get; set; }

        // 🔗 ligação com login
        public string IdentityUserId { get; set; } = string.Empty;

        public IdentityUser? IdentityUser { get; set; }

        // 📌 dados do aluno
        public string Name { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateTime? DOB { get; set; }

        public string? StudentNumber { get; set; }
    }
}