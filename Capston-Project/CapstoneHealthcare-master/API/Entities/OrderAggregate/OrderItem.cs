namespace APPUI.Entities.OrderAggregate
{
    public class OrderItem
    {
        public int Id { get; set; }
        public MedicineItemOrdered ItemOrdered { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
    }
}
