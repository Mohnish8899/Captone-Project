using APPUI.Entities.OrderAggregate;

namespace APPUI.DTO
{
    public class CreateOrderDto
    {
        public bool SaveAddress { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
    }
}
