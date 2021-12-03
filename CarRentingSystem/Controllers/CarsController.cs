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

      

        public IActionResult All([FromQuery]AllCarsQueryModel query)
        {
            var carQuery = this.data.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                carQuery = carQuery.Where(c => c.Brand == query.Brand);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                //in order to work well, the string must be splitted and each one must be looked for separately
                carQuery = carQuery.Where(c =>
                 (c.Brand + " " + c.Model).ToLower().Contains(query.SearchTerm.ToLower())
                || c.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            carQuery = query.Sorting switch
            {
                CarSorting.Year => carQuery.OrderByDescending(c => c.Year),
                CarSorting.BrandAndModel => carQuery.OrderBy(c => c.Brand).ThenBy(c => c.Model),
                CarSorting.DateCreated or _ => carQuery.OrderByDescending(c=>c.Id)
            };

            var totalCars = this.data.Cars.Count();

            var cars = carQuery
                .Skip((query.CurrentPage-1)*AllCarsQueryModel.CarsPerPage)
                .Take(AllCarsQueryModel.CarsPerPage)
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

            query.TotalCar = totalCars;
            query.Brands = carBrands;
            query.Cars = cars;

            return View(query);
        }

        public IActionResult Add()
          => View(new AddCarFormModel
          {
              Categories = this.GetCarCategories()
          });

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
