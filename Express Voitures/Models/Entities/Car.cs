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
        public int Year { get; set; }

        [Required]
        public int MakeId { get; set; }

        [Required]
        public int ModelId { get; set; }

        [StringLength(100)]
        public required string Trim { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        public DateTime? SaleDate { get; set; }

        public decimal? SalePrice { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        // Navigation properties
        [ForeignKey("MakeId")]
        public required Make Make { get; set; }

        [ForeignKey("ModelId")]
        public required Model Model { get; set; }

        public ICollection<Repair> Repairs { get; set; } = new List<Repair>();
    }
}
