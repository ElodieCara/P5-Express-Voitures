using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoitures.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        [Required]
        [Range(1990, 2023, ErrorMessage = "Year must be between 1990 and the current year.")]
        public int Year { get; set; }

        [Required]
        public int MakeId { get; set; }

        [Required]
        public int ModelId { get; set; }

        [StringLength(100)]
        public required string Trim { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SaleDate { get; set; }

        public decimal? SalePrice { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [StringLength(255)]
        public string? PhotoPath { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        // Navigation properties
        [ForeignKey("MakeId")]
        public required Make Make { get; set; }

        [ForeignKey("ModelId")]
        public required Model Model { get; set; }

        public ICollection<Repair> Repairs { get; set; } = new List<Repair>();
    }
}
