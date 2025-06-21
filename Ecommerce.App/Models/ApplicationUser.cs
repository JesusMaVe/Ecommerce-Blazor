using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.App.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; init; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string LastName { get; init; } = string.Empty;
        
        [Required]
        public UserType UserType { get; init; } = UserType.Customer;
        
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        
        public string FullName => $"{FirstName} {LastName}";
    }
    
    public enum UserType
    {
        Customer = 0,
        Seller = 1
    }
}