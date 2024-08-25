using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Modal
{
    [Table("tbl_historyData")]
    public class TblHistoryData
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        [Column("riverStationId")]
        public int RiverStationId { get; set; }

        [Column("riverHeight")]
        public double RiverHeight { get; set; }

        [Column("rainfallData")]
        public double RainfallData { get; set; }
        [Column("isactive")]
        public bool? Isactive { get; set; }
    }
}
