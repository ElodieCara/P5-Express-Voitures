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

        [ForeignKey("CarId")]
        public Car? Car { get; set; } 

        [Required]
        [StringLength(1000)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
        [Display(Name = "Coût")]
        public decimal Cost { get; set; }
    }
}
