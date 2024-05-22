using ExpressVoitures.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Repair
{
    [Key]
    public int RepairId { get; set; }

    [ForeignKey("Car")]
    public required string VIN { get; set; }

    public required Car Car { get; set; }

    [StringLength(255)]
    public required string Description { get; set; }

    public decimal Cost { get; set; }

    public DateTime Date { get; set; }

    [ForeignKey("User")]
    public required string UserId { get; set; }

    public required User User { get; set; }
}
