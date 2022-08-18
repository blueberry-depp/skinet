namespace API.Dtos
{
    // Data transfer object is a container basically for moving data between layers and a dto typically does not contain any business logic,
    // they only have simple setters and getters.
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        // Return a String instead of the actual object.
        public string ProductType { get; set; }
        // Return a String instead of the actual object.
        public string ProductBrand { get; set; }
    }
}
