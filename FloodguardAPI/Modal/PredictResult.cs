using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FloodguardAPI.Modal
{
    public class PredictResult
    {
        public int Id { get; set; }
        public string? RangeDate { get; set; } 
        public int ConfigTime { get; set; }
        public int RiverId { get; set; }
        public int StationId { get; set; }
        public double Rainfall { get; set; }
        public double RiverHeight { get; set; }
        public string Status { get; set; }
    }
}
