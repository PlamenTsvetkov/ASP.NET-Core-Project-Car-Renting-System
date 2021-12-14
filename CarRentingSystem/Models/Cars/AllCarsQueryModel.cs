namespace CarRentingSystem.Models.Cars
{

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CarRentingSystem.Services.Cars;

    public class AllCarsQueryModel
    {
        public const int CarsPerPage = 3;
        public string Brand { get; set; }
        public IEnumerable<string> Brands { get; set; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public CarSorting Sorting { get; init; }

        public int CurrentPage { get; set; } = 1;

        public IEnumerable<CarServiceModel> Cars { get; set; }
        public int TotalCar { get; set; }

    }
}
