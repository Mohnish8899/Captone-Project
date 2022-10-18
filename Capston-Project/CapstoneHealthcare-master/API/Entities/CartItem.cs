using System.ComponentModel.DataAnnotations.Schema;

namespace APPUI.Entities
{
    [Table("CartItems")]
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int MedicineId { get; set; }

        public Product Medicine { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }  
        
    }
}