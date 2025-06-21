using System.ComponentModel.DataAnnotations;
using Ecommerce.App.Models;

namespace Ecommerce.App.ViewModels
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a 0")]
        public int Stock { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        public int CategoryId { get; set; }
        
        public string? ImageUrl { get; set; }
    }
    
    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
        public string? Description { get; set; }
    }
    
    public class EditProductViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a 0")]
        public int Stock { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        public int CategoryId { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public bool IsActive { get; set; }
        
        public static EditProductViewModel FromProduct(Product product)
        {
            return new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive
            };
        }
    }
}