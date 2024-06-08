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

        [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN must be 17 characters.")]
        public string? VIN { get; set; }

        [Required]
        [Range(1990, 2023, ErrorMessage = "Year must be between 1990 and the current year.")]
        public int Year { get; set; }

        [Required]
        public int MakeId { get; set; }

        [Required]
        public int ModelId { get; set; }

        [StringLength(100)]
        public string? Trim { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
        public decimal PurchasePrice { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SaleDate { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
        public decimal? SalePrice { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [StringLength(255)]
        public string? PhotoPath { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AvailabilityDate { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
        public decimal RepairCost { get; set; }

        // Navigation properties
        [ForeignKey("MakeId")]
        public Make? Make { get; set; }

        [ForeignKey("ModelId")]
        public Model? Model { get; set; }

        public ICollection<Repair> Repairs { get; } = [];


        [NotMapped]
        public decimal CalculatedSalePrice
        {
            get
            {
                return PurchasePrice + RepairCost + 500;
            }
        }
    }

}
