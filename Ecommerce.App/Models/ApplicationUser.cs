using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.App.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        public UserType UserType { get; set; } = UserType.Customer;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public string FullName => $"{FirstName} {LastName}";
    }
    
    public enum UserType
    {
        Customer = 0,
        Seller = 1
    }
}