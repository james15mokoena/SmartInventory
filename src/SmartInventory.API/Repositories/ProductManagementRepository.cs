using SmartInventory.API.Data;
using SmartInventory.API.Domain.Models;

namespace SmartInventory.API.Repositories;

/// <summary>
/// Used to interact with the database.
/// </summary>
public class ProductManagementRepository(DatabaseContext context)
{
    /// <summary>
    /// Used to interact with the database.
    /// </summary>
    private readonly DatabaseContext _context = context;

    /// <summary>
    /// Used to add a new product in the database.
    /// </summary>
    /// <param name="newProduct"></param>
    /// <returns></returns>
    public bool CreateProduct(Product newProduct)
    {
        _context.Products.Add(newProduct);
        return _context.SaveChanges() > 0;
    }

    /// <summary>
    /// Used to fetch a product's details from the database.
    /// </summary>
    /// <param name="sku"></param>
    /// <returns></returns>
    public Product? GetProductBySku(string sku) => _context.Products.FirstOrDefault(p => p.SKU == sku);

    /// <summary>
    /// Used to fetch all active products.
    /// </summary>
    /// <returns></returns>
    public List<Product>? GetActiveProducts() => [.. _context.Products.Where(p => p.IsActive == true)];

    /// <summary>
    /// Used to fetch all deactivated products.
    /// </summary>
    /// <returns></returns>
    public List<Product>? GetDeactivatedProducts() => [.. _context.Products.Where(p => p.IsActive == false)];
}