using System.ComponentModel.DataAnnotations;

namespace BookRecommender.Models.ViewModels
{
    public class ReviewViewModel
    {
        // Hidden field in the form — tells the controller WHICH book this review is for
        public int BookId { get; set; }

        // 1-5 star rating, required, restricted to that range by [Range]
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        // Optional written review text
        public string? Comment { get; set; }
    }
}
