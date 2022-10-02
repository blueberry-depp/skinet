using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

public class AddressDto
{
    // DTO is a good thing to use to validate against bad data because this is when we receive it from the user.
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    public string Street { get; set; }
    
    [Required]
    public string City { get; set; }
    
    [Required]
    public string State { get; set; }
    
    [Required]
    public string Zipcode { get; set; }
}