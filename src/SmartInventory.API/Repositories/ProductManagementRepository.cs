using SmartInventory.API.Data;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;

namespace SmartInventory.API.Repositories;

/// <summary>
/// Used to interact with the database.
/// </summary>
public class ProductManagementRepository(DatabaseContext context, StockManagementRepository stockRepo,
                UserManagementRepository userRepo)
{
    /// <summary>
    /// Used to interact with the database.
    /// </summary>
    private readonly DatabaseContext _context = context;

    /// <summary>
    /// Used to interact with the stock management subsystem.
    /// </summary>
    private readonly StockManagementRepository _stockRepo = stockRepo;

    /// <summary>
    /// Used to interact with the user management subsystem.
    /// </summary>
    private readonly UserManagementRepository _userRepo = userRepo;

    /// <summary>
    /// Used to add a new product in the database.
    /// </summary>
    /// <param name="newProduct"></param>
    /// /// <param name="username"></param>
    /// <returns></returns>
    public bool CreateProduct(Product newProduct, string username)
    {
        _context.Products.Add(newProduct);

        if (_context.SaveChanges() > 0)
        {
            if (_userRepo.GetUserByUsername(username) is Admin admin)
            {
                if(_stockRepo.GetTransactionReasonId("Received") is int id && id >= 0)
                    return _stockRepo.RecordIncomingStock(newProduct.SKU, newProduct.CurrentStock, admin.Id, id,true);
            }
        }
        return false;
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

    /// <summary>
    /// Activates or deactivates a product.
    /// </summary>
    /// <param name="sku"></param>
    /// <returns></returns>
    public bool ToggleProductActiveStatus(string sku)
    {
        if (GetProductBySku(sku) is Product product)
        {
            product.IsActive = !product.IsActive;
            _context.Update(product);
            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Used to update a product's data.
    /// </summary>
    /// <param name="updatedProduct"></param>
    /// <returns></returns>
    public Product? EditProduct(ProductDto updatedProduct)
    {
        if (GetProductBySku(updatedProduct.SKU!) is Product product)
        {
            bool isUpdated = false;

            if (product.Name != updatedProduct.Name)
            {
                product.Name = updatedProduct.Name!;
                isUpdated = true;
            }

            if (product.Description != updatedProduct.Description)
            {
                product.Description = updatedProduct.Description!;
                isUpdated = true;
            }

            if (product.Category != updatedProduct.Category)
            {
                product.Category = updatedProduct.Category!;
                isUpdated = true;
            }

            if (product.CostPrice != updatedProduct.CostPrice)
            {
                product.CostPrice = updatedProduct.CostPrice!;
                isUpdated = true;
            }

            if (product.UnitPrice != updatedProduct.UnitPrice)
            {
                product.UnitPrice = updatedProduct.UnitPrice!;
                isUpdated = true;
            }

            if (product.CurrentStock != updatedProduct.CurrentStock)
            {
                product.CurrentStock = updatedProduct.CurrentStock!;
                isUpdated = true;
            }

            if (product.MinimumStockLevel != updatedProduct.MinimumStockLevel)
            {
                product.MinimumStockLevel = updatedProduct.MinimumStockLevel!;
                isUpdated = true;
            }

            if (product.ReorderQuantity != updatedProduct.ReorderQuantity)
            {
                product.ReorderQuantity = updatedProduct.ReorderQuantity!;
                isUpdated = true;
            }

            if (product.UnitMeasurement != updatedProduct.UnitMeasurement)
            {
                product.UnitMeasurement = updatedProduct.UnitMeasurement!;
                isUpdated = true;
            }

            if (isUpdated)
            {
                _context.Update(product);
                return _context.SaveChanges() > 0 ? product : null;
            }
        }
        return null;
    }
}