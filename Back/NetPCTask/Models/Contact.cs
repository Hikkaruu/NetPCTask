using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetPCTask.Models
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }


        public int SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }

        public string Phone { get; set; }

        public string Birth_date { get; set; }
    }
}
