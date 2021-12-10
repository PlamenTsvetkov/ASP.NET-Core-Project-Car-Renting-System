namespace CarRentingSystem.Controllers.Api
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using CarRentingSystem.Data;
    using CarRentingSystem.Models.Api.Statistics;

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly CarRentingDbContext data;

        public StatisticsApiController(CarRentingDbContext data)
            => this.data = data;

        [HttpGet]
        public StatisticsResponseModel GetStatistics()
        {
            var totalCars = this.data.Cars.Count();
            var totalUsers = this.data.Users.Count();

            return new StatisticsResponseModel
            {
                TotalCars = totalCars,
                TotalUsers = totalUsers,
                TotalRents = 0,
            };
        }
    }
}
