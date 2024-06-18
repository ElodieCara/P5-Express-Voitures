namespace ExpressVoitures.Models
{
    public class CarViewModel
    {
        public required string VIN { get; set; }
        public int Year { get; set; }
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public required string Trim { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime? AvailabilityDate { get; set; }
        public bool IsAvailable { get; set; }
        public required string PhotoUrl { get; set; }
        public required string Description { get; set; }
        public required ICollection<RepairViewModel> Repairs { get; set; }

        public decimal CalculatedSalePrice => PurchasePrice + Repairs.Sum(r => r.Cost) + 500;
    }

    public class RepairViewModel
    {
        public required string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime Date { get; set; }
    }
}
