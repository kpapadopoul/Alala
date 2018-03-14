using System.ComponentModel.DataAnnotations;

namespace AlalaWebAPI.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}