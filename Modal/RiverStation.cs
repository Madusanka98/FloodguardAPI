using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Modal
{
    
    public class RiverStation
    {
        public int Id { get; set; }
        public River? River { get; set; }

        [StringLength(20)]
        public string? Name { get; set; }
        [StringLength(50)]
        public string? Latitude { get; set; }
        [StringLength(50)]
        public string? Longitude { get; set; }

        public bool? Isactive { get; set; }

        public double AlertLevel { get; set; }
        public double MajorLevel { get; set; }
        public double MinorLevel { get; set; }
    }
}
