namespace CarRentingSystem.Models.Dealers
{
using System.ComponentModel.DataAnnotations;

using static Data.DataConstant.Dealer;
    public class BecomeDealerFormModel
    {
        public int Id { get; init; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; init; }

    }
}
