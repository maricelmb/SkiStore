using System.ComponentModel.DataAnnotations.Schema;

namespace SkiStore.Entities
{
    [Table("BasketItems")] //Table name in the db
    public class BasketItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        //navigation properties
        public int ProductId { get; set; }
        public required Product Product { get; set; }

        //navigation back to the basket
        //makes sure that if we delete the basket, all its items are deleted too (onDelete: Cascade)
        public int BasketId { get; set; }

        //can't use 'required' keyword here as it will require us to provide a value if Id in Product.AddItem
        // null! - null forgiving operator - suppresses warning w/o making it nullable
        public Basket Basket { get; set; } = null!;
    }
}
