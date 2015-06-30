namespace Core.Models
{
    public class ProductRelation
    {
        private ProductRelation() { }

        public ProductRelation(int mainProductId, int accessoryProductId)
        {
            MainProductId = mainProductId;
            AccessoryProductId = accessoryProductId;
        }

        public int MainProductId { get; private set; }
        public int AccessoryProductId { get; private set; }
    }
}