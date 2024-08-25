using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FloodguardAPI.Repos.Models
{
    [Table("tbl_riverStationUsers")]
    public class TblRiverStationUsers
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("riverStationId")]
        public int RiverStationId { get; set; }

        [Column("userId")]
        public int UserId { get; set; }
        [Column("isactive")]
        public bool? Isactive { get; set; }
    }
}
