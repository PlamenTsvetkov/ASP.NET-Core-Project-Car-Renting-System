namespace CarRentingSystem.Models.Api.Statistics
{
    using System.Collections.Generic;

    public class StatisticsResponseModel
    {
        public int TotalCars { get; init; }

        public int TotalUsers { get; init; }

        public int TotalRents { get; init; }
    }
}
