using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HouseRentingSystem.Core.Models.House
{
    public class HouseModel
    {
        public int Id { get; init; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; init; } = null!;

        [Required]
        [StringLength(150, MinimumLength = 30)]
        public string Address { get; init; } = null!;

        [Required]
        [StringLength(500, MinimumLength = 50)]
        public string Description { get; init; } = null!;

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; init; } = null!;

        [Required]
        [Display(Name = "Price per month")]
        [Range(0.00, 2000.00, ErrorMessage = "Price per month must be a positive number and less than {2} leva")]
        public decimal PricePerMonth { get; init; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        public IEnumerable<HouseCategoryModel> HouseCategories { get; init; } = new List<HouseCategoryModel>();
    }
}
