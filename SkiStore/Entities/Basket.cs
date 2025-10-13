namespace SkiStore.Entities
{
    public class Basket
    {
        public int Id { get; set; }

        //stored in cookie/local storage on client
        public required string BasketId { get; set; }

        public List<BasketItem> Items { get; set; } = [];

        #region Tracking the state of the item and has nothing to do with the db yet
        public void AddItem(Product product, int quantity)
        {
            if (product == null) ArgumentNullException.ThrowIfNull(product);

            if (quantity <= 0) throw new ArgumentOutOfRangeException("Quantity should be greater than 0.", nameof(quantity));

            var existingItem = FindItem(product.Id);

            if (existingItem == null)
            {
                Items.Add(new BasketItem
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                existingItem.Quantity += quantity;
            }
        }

        public void RemoveItem(int productId, int quantity)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException("Quantity should be greater than 0.", nameof(quantity));

            var item = FindItem(productId);

            if (item == null) return;
            item.Quantity -= quantity;

            if (item.Quantity <= 0) Items.Remove(item);
        }

        private BasketItem? FindItem(int id)
        {
            return Items.FirstOrDefault(item => item.ProductId == id);
        } 
        #endregion
    }
}
