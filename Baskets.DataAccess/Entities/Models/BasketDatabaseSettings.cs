namespace Baskets.DataAccess.Entities.Models
{
    public class BasketDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string BasketsCollectionName { get; set; } = null!;
        public string BasketItemsCollectionName { get; set; } = null!;
        public string ItemsCollectionName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
    }
}
