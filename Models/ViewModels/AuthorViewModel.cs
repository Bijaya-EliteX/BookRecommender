using System.ComponentModel.DataAnnotations;

namespace BookRecommender.Models.ViewModels
{
    public class AuthorViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}