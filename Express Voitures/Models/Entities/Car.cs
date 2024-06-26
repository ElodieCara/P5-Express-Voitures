using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoitures.Models
{
    // Classe représentant une voiture
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

        [ForeignKey("MakeId")]
        public Make? Make { get; set; }

        [ForeignKey("ModelId")]
        public Model? Model { get; set; }

        // Collection de réparations directement incluse dans la classe Car
        public ICollection<Repair> Repairs { get; set; } = new List<Repair>();

        // Propriété pour gérer l'état de la voiture
        [NotMapped]
        public CarStatus CarStatus => new CarStatus(this);

        // Propriété pour gérer les réparations de la voiture
        [NotMapped]
        public CarRepairs CarRepairs => new CarRepairs(this);
    }

    // Classe pour gérer l'état de la voiture
    public class CarStatus : IStatusOperations
    {
        private readonly Car _car;

        // Constructeur prenant un objet Car
        public CarStatus(Car car)
        {
            _car = car;
        }

        // Propriété calculée pour obtenir et définir le statut de la voiture
        [NotMapped]
        public string Status
        {
            get
            {
                if (_car.IsAvailable)
                {
                    return "Disponible";
                }
                else if (_car.SaleDate.HasValue)
                {
                    return "Vendu";
                }
                else
                {
                    // Si _car.IsAvailable est false et SaleDate n'a pas de valeur, alors nous retournons "Non Disponible".
                    return "Disponible";
                }
            }
            set
            {
                if (value == "Disponible")
                {
                    _car.IsAvailable = true;
                    _car.SaleDate = null;
                }
                else if (value == "Vendu")
                {
                    _car.IsAvailable = false;
                    _car.SaleDate = _car.SaleDate ?? DateTime.Now;
                }
                else
                {
                    _car.IsAvailable = false;
                    _car.SaleDate = null;
                }
            }

        }
    }

    // Classe pour gérer les réparations de la voiture
    public class CarRepairs : IRepairOperations
    {
        private readonly Car _car;

        // Constructeur prenant un objet Car
        public CarRepairs(Car car)
        {
            _car = car;
        }

        // Collection de réparations
        public ICollection<Repair> Repairs => _car.Repairs;

        // Propriété calculée pour obtenir la description des réparations
        [NotMapped]
        [Display(Name = "Descriptif des réparations")]
        public string RepairDescriptions => string.Join(", ", Repairs.Select(r => r.RepairDescription));

        // Propriété calculée pour obtenir le coût total des réparations
        [NotMapped]
        [Display(Name = "Coût total des réparations")]
        public decimal TotalRepairCost => Repairs.Sum(r => r.Cost);
    }

    // Interfaces pour respecter le principe de Ségrégation des Interfaces
    public interface IStatusOperations
    {
        string Status { get; set; }
    }

    public interface IRepairOperations
    {
        ICollection<Repair> Repairs { get; }
        string RepairDescriptions { get; }
        decimal TotalRepairCost { get; }
    }
}
