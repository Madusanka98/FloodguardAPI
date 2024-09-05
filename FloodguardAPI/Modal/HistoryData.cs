using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Modal
{
    
    public class HistoryData
    {
        
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        public int RiverStationId { get; set; }

        public double RiverHeight { get; set; }

        public double RainfallData { get; set; }

        public bool? Isactive { get; set; }
    }
}
