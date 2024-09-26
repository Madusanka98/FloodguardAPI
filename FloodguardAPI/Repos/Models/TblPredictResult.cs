using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloodguardAPI.Repos.Models
{
    [Table("tbl_predictResult")]
    public class TblPredictResult
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Change DateRange to string type with nvarchar in SQL
        [Column(TypeName = "nvarchar(100)")]
        public string RangeDate { get; set; } // String type for storing the date range

        [Column("ConfigTime")]
        public int ConfigTime { get; set; }

        [Column("RiverId")]
        public int RiverId { get; set; }

        [Column("StationId")]
        public int StationId { get; set; }

        [Column("Rainfall")]
        public double Rainfall { get; set; }

        [Column("RiverHeight")]
        public double RiverHeight { get; set; }

        [Column("Status")]
        public string Status { get; set; }
    }
}
