using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LearnAPI.Modal
{
    public class River
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }

        public bool? Isactive { get; set; }
    }
}
