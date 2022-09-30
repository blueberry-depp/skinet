using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Identity
{
    // One user is going to have one address.
    public class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }

        // Configure one to one relationship using entity framework conventions.
        // AppUserId: foreign key of AppUser table. and it's string and by default it's null in entity framework. We don't want allow this to be null.
        [Required]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}