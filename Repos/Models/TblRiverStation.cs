using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Modal
{
    [Table("tbl_riverStation")]
    public class TblRiverStation
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("riverId")]
        public int RiverId { get; set; }

        [Column("name")]
        [StringLength(50)]
        public string? Name { get; set; }

        [Column("latitude")]
        [StringLength(50)]
        public string? Latitude { get; set; } 

        [Column("longitude")]
        [StringLength(50)]
        public string? Longitude { get; set; }

        [Column("isactive")]
        public bool? Isactive { get; set; }
    }
}
