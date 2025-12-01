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
}