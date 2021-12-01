namespace CarRentingSystem.Controllers
{
    using System.Linq;
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    using CarRentingSystem.Models;
    using CarRentingSystem.Data;
    using CarRentingSystem.Models.Cars;

    public class HomeController : Controller
    {
        private readonly CarRentingDbContext data;

        public HomeController(CarRentingDbContext data)
            => this.data = data;
        public IActionResult Index()
        {
            var cars = this.data
               .Cars
               .OrderByDescending(c => c.Id)
               .Select(c => new CarListingViewModela
               {
                   Id = c.Id,
                   Brand = c.Brand,
                   Model = c.Model,
                   Year = c.Year,
                   ImageUrl = c.ImageUrl,
                   Category = c.Category.Name,
               })
               .Take(3)
               .ToList();

            return View(cars);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
