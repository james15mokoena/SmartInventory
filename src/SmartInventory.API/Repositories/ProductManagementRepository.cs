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
        if (GetProductBySku(newProduct.SKU) is null)
        {
            _context.Products.Add(newProduct);

            if (_context.SaveChanges() > 0)
            {
                if (_userRepo.GetUserByUsername(username) is Staff staff)
                {
                    // create a transaction
                    StockTransaction transaction = new()
                    {
                        ProductId = newProduct.SKU
                        ,
                        NewStock = newProduct.CurrentStock
                        ,
                        PreviousStock = 0
                        ,
                        QuantityChange = 0
                        ,
                        TransactionId = 0
                        ,
                        Product = newProduct
                        ,
                        Date = DateTime.Now
                        ,
                        ReasonTypeId = _stockRepo.GetTransactionReasonId("New Product")
                        ,
                        UserId = staff.Id
                    };

                    _context.StockTransactions.Add(transaction);

                    if (_context.SaveChanges() > 0)
                        return true;
                    else
                    {
                        _context.Products.Remove(newProduct);
                        return _context.SaveChanges() > 0;
                    }
                }
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
    /// Fetches a product by its name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Product? GetProductByName(string name) => _context.Products.FirstOrDefault(p => p.Name == name);

    /// <summary>
    /// Fetches all the products that belong to the specified category.
    /// </summary>
    /// <returns></returns>
    public List<Product>? GetProductsByCategory(string category) => [.. _context.Products.Where(p => p.Category == category)];

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

            if (product.Name != updatedProduct.Name && updatedProduct.Name != "string")
            {
                product.Name = updatedProduct.Name!;
                isUpdated = true;
            }

            if (product.Description != updatedProduct.Description && updatedProduct.Description != "string")
            {
                product.Description = updatedProduct.Description!;
                isUpdated = true;
            }

            if (product.Category != updatedProduct.Category && updatedProduct.Category != "string")
            {
                product.Category = updatedProduct.Category!;
                isUpdated = true;
            }

            if (product.CostPrice != updatedProduct.CostPrice && updatedProduct.CostPrice > 0)
            {
                product.CostPrice = updatedProduct.CostPrice;
                isUpdated = true;
            }

            if (product.UnitPrice != updatedProduct.UnitPrice && updatedProduct.UnitPrice > 0)
            {
                product.UnitPrice = updatedProduct.UnitPrice;
                isUpdated = true;
            }

            if (product.CurrentStock != updatedProduct.CurrentStock && updatedProduct.CurrentStock > 0)
            {
                product.CurrentStock = updatedProduct.CurrentStock;
                isUpdated = true;
            }

            if (product.MinimumStockLevel != updatedProduct.MinimumStockLevel && updatedProduct.MinimumStockLevel > 0)
            {
                product.MinimumStockLevel = updatedProduct.MinimumStockLevel;
                isUpdated = true;
            }

            if(product.MaximumStockLevel != updatedProduct.MaximumStockLevel && updatedProduct.MaximumStockLevel > 0)
            {
                product.MaximumStockLevel = updatedProduct.MaximumStockLevel;
                isUpdated = true;
            }

            if (product.ReorderQuantity != updatedProduct.ReorderQuantity && updatedProduct.ReorderQuantity > 0)
            {
                product.ReorderQuantity = updatedProduct.ReorderQuantity;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(updatedProduct.UnitMeasurement) && product.UnitMeasurement != updatedProduct.UnitMeasurement)
            {
                product.UnitMeasurement = updatedProduct.UnitMeasurement;
                isUpdated = true;
            }

            if (product.ImageUrl != updatedProduct.ImageUrl && product.ImageUrl != "")
            {
                product.ImageUrl = updatedProduct.ImageUrl!;
                isUpdated = true;
            }

            if (product.Barcode != updatedProduct.Barcode && product.Barcode != "")
            {
                product.Barcode = updatedProduct.Barcode!;
                isUpdated = true;
            }

            if(product.IsActive != updatedProduct.IsActive)
            {
                product.IsActive = updatedProduct.IsActive;
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