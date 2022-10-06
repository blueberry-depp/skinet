namespace API.Dtos;

public class OrderDto
{
    public string BasketId { get; set; }
    public int DeliveryMethodId { get; set; }
    // Dto that we're need to map into the address inside order aggregates. We need create mapping profile for this.
    public AddressDto ShipToAddress { get; set; }
    
}