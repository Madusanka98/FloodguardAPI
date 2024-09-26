namespace FloodguardAPI.Modal
{
    public class PredictResultDto
    {
            public int Id { get; set; }
            public string? RangeDate { get; set; }
            public int ConfigTime { get; set; }
            public string RiverName { get; set; }
            public string StationName { get; set; } 
            public double Rainfall { get; set; }
            public double RiverHeight { get; set; }
            public string Status { get; set; }
        
    }
}
