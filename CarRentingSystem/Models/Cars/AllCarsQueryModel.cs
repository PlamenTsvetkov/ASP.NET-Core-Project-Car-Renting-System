namespace CarRentingSystem.Models.Cars
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllCarsQueryModel
    {
        public string Brand { get; set; }
        public IEnumerable<string> Brands { get; init; }

        [Display(Name ="Search by text:")]
        public string SearchTerm { get; init; }

        public CarSorting CarSorting { get; init; }

        public IEnumerable<CarListingViewModela> Cars { get; init; }
        
    }
}
