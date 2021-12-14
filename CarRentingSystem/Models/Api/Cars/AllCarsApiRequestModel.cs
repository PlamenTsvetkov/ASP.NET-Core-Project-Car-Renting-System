namespace CarRentingSystem.Models.Api.Cars
{

using System.Collections.Generic;

    public class AllCarsApiRequestModel
    {
        public string Brand { get; set; }
        public string SearchTerm { get; init; }

        public CarSorting Sorting { get; init; }
        public int CurrentPage { get; set; } = 1;

        public int CarsPerPage { get; init; } = 10;
    }
}
