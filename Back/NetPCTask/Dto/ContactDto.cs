using NetPCTask.Models;
using System.ComponentModel.DataAnnotations;

namespace NetPCTask.Dto
{
    public class ContactDto
    {
        public int Id { get; set; }


        public string Name { get; set; }


        public string Surname { get; set; }


        public string Email { get; set; }


        public string Password { get; set; }

        public string Phone { get; set; }


        public string Birth_date { get; set; }

        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
    }
}
