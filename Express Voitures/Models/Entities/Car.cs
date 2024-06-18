using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ExpressVoitures.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        public string? VIN { get; set; }

        [Required]
        [Range(1990, 2023, ErrorMessage = "Year must be between 1990 and the current year.")]
        [Display(Name = "Année")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Marque")]
        public int MakeId { get; set; }

        [Required]
        [Display(Name = "Modèle")]
        public int ModelId { get; set; }

        [StringLength(100)]
        [Display(Name = "Finition")]
        public string? Trim { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date d’achat")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
        [Display(Name = "Prix d’achat")]
        public decimal PurchasePrice { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date de vente")]
        public DateTime? SaleDate { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
        [Display(Name = "Prix de vente")]
        public decimal? SalePrice { get; set; }

        [Required]
        [Display(Name = "Disponible")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Photo")]
        public string? PhotoPath { get; set; }

        [StringLength(1000)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date de disponibilité à la vente")]
        public DateTime AvailabilityDate { get; set; }

        [NotMapped]
        public string Status
        {
            get
            {
                if (IsAvailable)
                {
                    return "Disponible";
                }
                else if (SaleDate.HasValue)
                {
                    return "Vendu";
                }
                else
                {
                    return "Non Disponible";
                }
            }
            set
            {
                if (value == "Disponible")
                {
                    IsAvailable = true;
                    SaleDate = null;
                }
                else if (value == "Vendu")
                {
                    IsAvailable = false;
                    SaleDate = DateTime.Now;
                }
                else if (value == "Non Disponible")
                {
                    IsAvailable = false;
                    SaleDate = null;
                }
            }
        }

        [ForeignKey("MakeId")]
        public Make? Make { get; set; }

        [ForeignKey("ModelId")]
        public Model? Model { get; set; }

        public ICollection<Repair> Repairs { get; } = new List<Repair>();

        [NotMapped]
        [Display(Name = "Descriptif des réparations")]
        public string RepairDescriptions => string.Join(", ", Repairs.Select(r => r.RepairDescription));

        [NotMapped]
        [Display(Name = "Coût total des réparations")]
        public decimal TotalRepairCost => Repairs.Sum(r => r.Cost);

        [NotMapped]
        public decimal CalculatedSalePrice => PurchasePrice + TotalRepairCost + 500;
    }
}
