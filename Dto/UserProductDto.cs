using Crud.Model;

namespace Crud.Dto
{
    public class UserProductDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<Product1Dto> Products { get; set; }

    }
}
