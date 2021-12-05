namespace CarRentingSystem.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstant.Dealer;
    public class Dealer
    {
        public Dealer()
        {
            this.Cars = new HashSet<Car>();
        }
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }

        public ICollection<Car> Cars { get; init; }

    }
}
