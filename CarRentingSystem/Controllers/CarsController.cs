namespace CarRentingSystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CarRentingSystem.Models.Cars;
    using System.Collections.Generic;
    using CarRentingSystem.Data;
    using System.Linq;
    using CarRentingSystem.Data.Models;

    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;

        public CarsController(CarRentingDbContext data)
            => this.data = data;

        public IActionResult Add()
            => View(new AddCarFormModel
            {
                Categories = this.GetCarCategories()
            });

        public IActionResult All(string brand, string searchTerm)
        {
            var carQuery = this.data.Cars.AsQueryable();
            if (!string.IsNullOrWhiteSpace(brand))
            {
                carQuery = carQuery.Where(c => c.Brand == brand);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                //in order to work well, the string must be splitted and each one must be looked for separately
                carQuery = carQuery.Where(c =>
                 (c.Brand + " " + c.Model).ToLower().Contains(searchTerm.ToLower())
                || c.Description.ToLower().Contains(searchTerm.ToLower()));
            }
            var cars = carQuery
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
                .ToList();

            var carBrands = this.data
                .Cars
                .Select(c => c.Brand)
                .Distinct()
                .OrderBy(br => br)
                .ToList();

            return View(new AllCarsQueryModel
            {
                Brands=carBrands,
                Cars = cars,
                SearchTerm = searchTerm
            });
        }


        [HttpPost]
        public IActionResult Add(AddCarFormModel car)
        {
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
            };

            this.data.Cars.Add(carData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));

        }
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
