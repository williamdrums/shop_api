using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este Campo deve conter entre 3 e 60 caracterés")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracterés")]
        public string Title { get; set; }

        [MaxLength(1024, ErrorMessage = "Este campo deve conter no maximo 1024 caractéres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Categoria Inválida")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}