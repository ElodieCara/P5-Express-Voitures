using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoitures.Models
{
    public class Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModelId { get; set; }

        [Required]
        public int MakeId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        // Navigation properties
        [ForeignKey("MakeId")]
        public Make? Make { get; set; }
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
