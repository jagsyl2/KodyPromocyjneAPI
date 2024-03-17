using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KodyPromocyjneAPI.DataLayer.Models
{
    public class PromoCode
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        public string Name { get; set; }
        public Guid Code { get; set; } = Guid.NewGuid();

        [Range(1, int.MaxValue, ErrorMessage = "Ilość możliwych użyć musi być większa niż zero")]
        [DefaultValue(0)]
        public int NumberOfPossibleUses { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}
