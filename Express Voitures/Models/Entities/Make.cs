using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Models
{
    public class Make
    {
        [Key]
        public int MakeId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        // Navigation property
        public required ICollection<Model> Models { get; set; }

        // Ajout de cette ligne pour que la relation avec Car soit bidirectionnelle
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
