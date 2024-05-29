using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoitures.Models
{
    public class Model
    {
        [Key]
        public int ModelId { get; set; }

        [Required]
        public int MakeId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        // Navigation properties
        [ForeignKey("MakeId")]
        public required Make Make { get; set; }
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
