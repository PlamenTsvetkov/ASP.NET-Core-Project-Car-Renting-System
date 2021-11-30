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

        public IActionResult Add() => View(new AddCarFormModel
        {
            Categories = this.GetCarCategories()
        });

       [HttpPost]
       public IActionResult Add(AddCarFormModel car)
        {
            if (!this.data.Categories.Any(c=>c.Id==car.CategoryId))
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

            return RedirectToAction("Index", "Home");
            
        }
        private IEnumerable<CarCategoryViewModel> GetCarCategories()
                => this.data
                 .Categories
                 .Select(c => new CarCategoryViewModel
                 {
                     Id=c.Id,
                     Name=c.Name
                 })
                 .ToList();
    }
}
