using System.ComponentModel.DataAnnotations.Schema;

namespace FloodguardAPI.Modal
{
    public class RiverStationUsers
    {
        public int Id { get; set; }

        public int RiverStationId { get; set; }

        public int UserId { get; set; }
        public bool? Isactive { get; set; }
    }
}
