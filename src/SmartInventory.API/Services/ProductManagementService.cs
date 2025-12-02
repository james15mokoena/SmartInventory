using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;
using SmartInventory.API.Repositories;

namespace SmartInventory.API.Services;

public class ProductManagementService(ProductManagementRepository productRepo, SupplierManagementService suppService)
{
    /// <summary>
    /// Used to interact with the database.
    /// </summary>
    private readonly ProductManagementRepository _productRepo = productRepo;

    /// <summary>
    /// Used to interact with the supplier management subsystem.
    /// </summary>
    private readonly SupplierManagementService _supplierService = suppService;

    /// <summary>
    /// Adds a new product.
    /// </summary>
    /// <param name="newProduct"></param>
    /// <returns></returns>
    public bool AddProduct(ProductDto newProduct)
    {
        if (!string.IsNullOrEmpty(newProduct.SKU) && !string.IsNullOrEmpty(newProduct.Name) &&
            !string.IsNullOrEmpty(newProduct.Description) && !string.IsNullOrEmpty(newProduct.Category) &&
            newProduct.CostPrice >= 0.0 && newProduct.UnitPrice >= 0.0 && newProduct.CurrentStock >= 0 &&
            newProduct.DateCreated != default && newProduct.LastUpdated != default && newProduct.MinimumStockLevel >= 0 &&
            newProduct.ReorderQuantity >= 0 && newProduct.UnitMeasurement >= 0.0 && newProduct.SupplierId >= 0)
        {
            // get the supplier of this product.
            Supplier? supplier = _supplierService.GetSupplier(newProduct.SupplierId);

            if (supplier != null)
            {
                return _productRepo.CreateProduct(new()
                {
                    SKU = newProduct.SKU,
                    Name = newProduct.Name,
                    Category = newProduct.Category,
                    UnitPrice = newProduct.UnitPrice,
                    CostPrice = newProduct.CostPrice,
                    CurrentStock = newProduct.CurrentStock,
                    IsActive = newProduct.IsActive,
                    MinimumStockLevel = newProduct.MinimumStockLevel,
                    DateCreated = newProduct.DateCreated,
                    LastUpdated = newProduct.LastUpdated,
                    Description = newProduct.Description,
                    ReorderQuantity = newProduct.ReorderQuantity,
                    UnitMeasurement = newProduct.UnitMeasurement,
                    SupplierId = newProduct.SupplierId,
                    Barcode = newProduct.Barcode ?? "",
                    Supplier = supplier,
                    PurchaseOrderItems = [],
                    StockTransactions = []
                });
            }
        }

        return false;
    }
    
    /// <summary>
    /// Gets a product identified by the SKU(Stock-Keeping Unit) number.
    /// </summary>
    /// <param name="sku"></param>
    /// <returns></returns>
    public ProductDto? GetProductBySku(string sku)
    {
        if (!string.IsNullOrEmpty(sku))
        {
            Product? product = _productRepo.GetProductBySku(sku);
            if (product != null)
            {
                return new ProductDto()
                {
                    SKU = product.SKU
                    ,
                    Barcode = product.Barcode
                    ,
                    Category = product.Category
                    ,
                    CostPrice = product.CostPrice
                    ,
                    CurrentStock = product.CurrentStock
                    ,
                    DateCreated = product.DateCreated
                    ,
                    Description = product.Description
                    ,
                    IsActive = product.IsActive
                    ,
                    LastUpdated = product.LastUpdated
                    ,
                    MinimumStockLevel = product.MinimumStockLevel
                    ,
                    Name = product.Name
                    ,
                    ReorderQuantity = product.ReorderQuantity
                    ,
                    SupplierId = product.SupplierId
                    ,
                    UnitMeasurement = product.UnitMeasurement
                    ,
                    UnitPrice = product.UnitPrice
                };
            }
        }
        return null;
    }
}