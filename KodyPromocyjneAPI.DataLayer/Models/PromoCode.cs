using System.ComponentModel.DataAnnotations;

namespace KodyPromocyjneAPI.DataLayer.Models
{
    public class PromoCode
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public Guid Code { get; set; }

        [Required]
        public int NumberOfPossibleUses { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
