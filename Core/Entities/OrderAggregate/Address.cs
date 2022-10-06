namespace Core.Entities.OrderAggregate;

// It's not going to be related to our order in any way other than the fact that the order owns it. And that means
// we're not going to drive from base entity, so we're not gonna give it an ID.
// This all class is considered as a supporting class for order itself. This is a kind of entity
// that's considered a value entity, it's going to be owned by order. So it doesn't have an ID but we do give
// it a constructor with the properties inside this particular class.
public class Address
{
    // Entity framework needs a parameter list constructor, otherwise we're going to get some complaints when we try and add a new migration,
    // and this time we'll just use the parameters. The parameter in this constructor is for entity framework because it won't be able to deal with this 
    // if it needs parameters to construct the address class itself.
    public Address()
    {
    }
    
    // This allow us to populate the fields when we create a new instance of order with the address.
    public Address(string firstName, string lastName, string street, string city, string state, string zipcode)
    {
        FirstName = firstName;
        LastName = lastName;
        Street = street;
        City = city;
        State = state;
        Zipcode = zipcode;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zipcode { get; set; }
}