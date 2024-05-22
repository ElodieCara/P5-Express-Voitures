using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Car
{
    [Key]
    [StringLength(17)]
    public required string VIN { get; set; }

    public int Year { get; set; }

    [StringLength(50)]
    public required string Brand { get; set; }

    [StringLength(50)]
    public required string Model { get; set; }

    [StringLength(50)]
    public required string Trim { get; set; }

    public DateTime PurchaseDate { get; set; }

    public decimal PurchasePrice { get; set; }

    public DateTime? AvailabilityDate { get; set; }

    public bool IsAvailable { get; set; } = true;

    [StringLength(255)]
    public required string PhotoUrl { get; set; }

    [StringLength(255)]
    public required string Description { get; set; }

    public required virtual ICollection<Repair> Repairs { get; set; }
    public required virtual ICollection<Sale> Sales { get; set; }
}
