namespace Supermarket.Domain.StoredProducts;

public record struct StoredProductId(int StoragePlaceId, int SupermarketId, int ProductId);