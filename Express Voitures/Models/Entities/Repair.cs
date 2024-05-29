using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoitures.Models
{
    public class Repair
    {
        [Key]
        public int RepairId { get; set; }

        [Required]
        public int CarId { get; set; }

        [Required]
        [StringLength(255)]
        public required string Description { get; set; }

        [Required]
        public decimal Cost { get; set; }

        // Navigation property
        [ForeignKey("CarId")]
        public required Car Car { get; set; }
    }
}
