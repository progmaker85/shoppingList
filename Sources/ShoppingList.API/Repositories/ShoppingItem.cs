using System;

namespace ShoppingList.API.Repositories
{
    public class ShoppingItem
    {
        public long Id { get; set; }
        public string ItemName { get; set; }
        public float? Quantity { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? CheckOffDate { get; set; }
    }
}