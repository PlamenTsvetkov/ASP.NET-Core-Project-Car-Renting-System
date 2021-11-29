namespace CarRentingSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstant;
    public class Category
    {
        public Category()
        {
            this.Cars = new HashSet<Car>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
