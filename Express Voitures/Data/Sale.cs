using ExpressVoitures.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Sale
{
    [Key]
    public int SaleId { get; set; }

    [ForeignKey("Car")]
    public required string VIN { get; set; }

    public required Car Car { get; set; }

    public decimal SalePrice { get; set; }

    public DateTime SaleDate { get; set; }

    [ForeignKey("User")]
    public required string UserId { get; set; }

    public required User User { get; set; }
}
