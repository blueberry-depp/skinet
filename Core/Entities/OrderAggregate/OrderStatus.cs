using System.Runtime.Serialization;

namespace Core.Entities.OrderAggregate;

// This is going to track the status order.
public enum OrderStatus
{   
    // We want to receive the text of this so we give the attribute.
    [EnumMember(Value = "Pending")]
    // When the order is first submitted.
    Pending,
    
    [EnumMember(Value = "Payment Received")]
    PaymentReceived,
    
    [EnumMember(Value = "Payment Failed")]
    PaymentFailed
    
}