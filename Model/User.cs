using System.ComponentModel.DataAnnotations;

namespace Casino_Project.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public double Balance { get; set; }
    }
}
