using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get;set; }
        public  string? ProductDescription { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")] 
        public User users { get; set; }
    }
    public class ProductDto
    {
   
        public int Id { get; set; }
        public string ProductName { get;set; }
        public  string? ProductDescription { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public int UserId { get; set; }
        
      
    }
    public class Product1Dto
    {

        public int Id { get; set; }
        public string ProductName { get; set; }
        public string? ProductDescription { get; set; } = string.Empty;
        public int ProductPrice { get; set; }





    }

}
