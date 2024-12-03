using System.ComponentModel.DataAnnotations;

namespace Crud.Model
{
    public class Driver
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
    }
}
