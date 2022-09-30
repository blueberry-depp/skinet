namespace API.Dtos
{
    public class UserDto
    {
        public string Email { get; set; }
        // This is the information we want to display on navbar in the angular application.
        public string DisplayName { get; set; }
        public string Token { get; set; }
    }
}