namespace CarRentingSystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CarRentingSystem.Models.Cars;
    using System.Collections.Generic;
    using CarRentingSystem.Data;
    using System.Linq;
    using CarRentingSystem.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using CarRentingSystem.Infrastucture;
    using CarRentingSystem.Services.Cars;

    public class CarsController : Controller
    {

        private readonly CarRentingDbContext data;
        private readonly ICarService cars;

        public CarsController(ICarService cars, CarRentingDbContext data)
        {
            this.cars = cars;
            this.data = data;
        }



        public IActionResult All([FromQuery] AllCarsQueryModel query)
        {
            var queryResult
                = this.cars.All(
                 query.Brand,
                   query.SearchTerm,
                   query.Sorting,
                   query.CurrentPage,
                   AllCarsQueryModel.CarsPerPage);

            var carBrands = this.cars.AllCarBrands();

            query.TotalCar = queryResult.TotalCar;
            query.Brands = carBrands;
            query.Cars = queryResult.Cars;

            return View(query);
        }
        [Authorize]
        public IActionResult Add()
        {
            if (this.UserIsDealer())
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            return View(new AddCarFormModel
            {
                Categories = this.GetCarCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddCarFormModel car)
        {
            var dealerId = this.data
                .Dealers
                .Where(d => d.UserId == this.User.GetId())
                .Select(d => d.Id)
                .FirstOrDefault();

            if (dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            if (!this.data.Categories.Any(c => c.Id == car.CategoryId))
            {
                this.ModelState.TryAddModelError(nameof(car.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                car.Categories = this.GetCarCategories();
                return View(car);
            }

            var carData = new Car
            {
                Brand = car.Brand,
                Model = car.Model,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                CategoryId = car.CategoryId,
                Year = car.Year,
                DealerId = dealerId
            };

            this.data.Cars.Add(carData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));

        }
        private bool UserIsDealer()
            => !this.data
                .Dealers
                .Any(d => d.UserId == this.User.GetId());

        private IEnumerable<CarCategoryViewModel> GetCarCategories()
                => this.data
                 .Categories
                 .Select(c => new CarCategoryViewModel
                 {
                     Id = c.Id,
                     Name = c.Name
                 })
                 .ToList();
    }
}
