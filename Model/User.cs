using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Sieve(CanFilter=true,CanSort=true)]
        public string name { get; set; }
        [Sieve(CanFilter = true, CanSort = true)]


        public string email { get; set; }
        [Sieve(CanFilter = true, CanSort = true)]

        public string password { get; set; }
        public string address { get; set; }


        public ICollection<Product> Products { get; set; }
    }
    public class UserDto
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string address { get; set; }

    }
}
