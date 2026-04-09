using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;
using SmartInventory.API.Repositories;

namespace SmartInventory.API.Services;

public class ProductManagementService(ProductManagementRepository productRepo, SupplierManagementService suppService,
    PermissionManagementService permServ)
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
    /// Used to interact with the permission management subsystem.
    /// </summary>
    private readonly PermissionManagementService _permService = permServ;

    /// <summary>
    /// Adds a new product.
    /// </summary>
    /// <param name="newProduct"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    public bool AddProduct(ProductDto newProduct, string username)
    {
        if (!string.IsNullOrEmpty(newProduct.SKU) && !string.IsNullOrEmpty(newProduct.Name) &&
            !string.IsNullOrEmpty(newProduct.Description) && !string.IsNullOrEmpty(newProduct.Category) &&
            newProduct.CostPrice >= 0.0 && newProduct.UnitPrice >= 0.0 && newProduct.CurrentStock >= 0 &&
            newProduct.DateCreated != default && newProduct.LastUpdated != default && newProduct.MinimumStockLevel >= 0 &&
            newProduct.ReorderQuantity >= 0 && newProduct.UnitMeasurement != null && newProduct.SupplierId >= 0 &&
            !string.IsNullOrEmpty(username) && newProduct.MaximumStockLevel >= 0 && _permService.IsAuthorized(username,"AddProduct"))
        {
            // get the supplier of this product.

            if (_supplierService.GetSupplier(newProduct.SupplierId) is Supplier supplier)
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
                    MaximumStockLevel = newProduct.MinimumStockLevel <= newProduct.MaximumStockLevel ?
                                        newProduct.MaximumStockLevel : newProduct.MinimumStockLevel + 5,
                    DateCreated = newProduct.DateCreated,
                    LastUpdated = newProduct.LastUpdated,
                    Description = newProduct.Description,
                    ReorderQuantity = newProduct.ReorderQuantity,
                    UnitMeasurement = newProduct.UnitMeasurement,
                    SupplierId = newProduct.SupplierId,
                    Barcode = newProduct.Barcode ?? "",
                    Supplier = supplier,
                    PurchaseOrderItems = [],
                    StockTransactions = [],
                    OrderItems = [],
                    InvoiceItems = [],
                    ImageUrl = newProduct.ImageUrl ?? ""
                }, username);                
            }
        }

        return false;
    }

    /// <summary>
    /// Gets a product identified by the SKU(Stock-Keeping Unit) number.
    /// </summary>
    /// <param name="sku"></param>
    /// <returns></returns>
    public ProductDto? GetProductBySku(string sku, string username)
    {
        if (!string.IsNullOrEmpty(sku) && _permService.IsAuthorized(username,"ViewProductDetails"))
        {
            if (_productRepo.GetProductBySku(sku) is Product product)
            {
                return new ProductDto()
                {
                    SupplierName = _supplierService.GetSupplier(product.SupplierId)!.SupplierName
                    ,
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
                    MaximumStockLevel = product.MaximumStockLevel
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
                    ,
                    ImageUrl = product.ImageUrl
                };
            }
        }
        return null;
    }

    /// <summary>
    /// Gets a product identified by the given name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public ProductDto? GetProductByName(string name, string username)
    {
        if (!string.IsNullOrEmpty(name) && _permService.IsAuthorized(username, "ViewProductDetails"))
        {
            if (_productRepo.GetProductByName(name) is Product product)
            {
                return new ProductDto()
                {
                    SupplierName = _supplierService.GetSupplier(product.SupplierId)!.SupplierName
                    ,
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
                    MaximumStockLevel = product.MaximumStockLevel
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
                    ,
                    ImageUrl = product.ImageUrl
                };
            }
        }
        return null;
    }
    
    /// <summary>
    /// Fetches all the products that belong to the specified category.
    /// </summary>
    /// <returns></returns>
    public List<ProductDto>? GetProductsByCategory(string category, string username)
    {
        if(!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(username) && _permService.IsAuthorized(username, "ViewProducts"))
        {
            List<Product>? products = _productRepo.GetProductsByCategory(category);

            if (products != null && products.Count > 0)
            {
                List<ProductDto>? dtos = [];

                foreach (Product product in products)
                {
                    dtos.Add(new()
                    {
                        SupplierName = _supplierService.GetSupplier(product.SupplierId)!.SupplierName
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
                        ImageUrl = product.ImageUrl
                        ,
                        IsActive = product.IsActive
                        ,
                        LastUpdated = product.LastUpdated
                        ,
                        MaximumStockLevel = product.MaximumStockLevel
                        ,
                        MinimumStockLevel = product.MinimumStockLevel
                        ,
                        Name = product.Name
                        ,
                        ReorderQuantity = product.ReorderQuantity
                        ,
                        SKU = product.SKU
                        ,
                        SupplierId = product.SupplierId
                        ,
                        UnitMeasurement = product.UnitMeasurement
                        ,
                        UnitPrice = product.UnitPrice
                    });
                }

                return dtos;
            }
        }
        return null;
    }

    /// <summary>
    /// Fetches all active products.
    /// </summary>
    /// <returns></returns>
    public List<ProductDto>? GetActiveProducts(string username)
    {

        if (_permService.IsAuthorized(username,"ViewProducts") && _productRepo.GetActiveProducts() is List<Product> products && products.Count > 0)
        {
            List<ProductDto> productDtos = [];

            foreach (Product product in products)
            {
                if (product != null)
                {
                    productDtos.Add(new ProductDto
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
                    });
                }
            }

            return productDtos;
        }

        return null;
    }

    /// <summary>
    /// Fetches all deactivated products.
    /// </summary>
    /// <returns></returns>
    public List<ProductDto>? GetDeactivatedProducts(string username)
    {

        if(_permService.IsAuthorized(username,"ViewProducts")  && _productRepo.GetDeactivatedProducts() is List<Product> products && products.Count > 0)
        {
            List<ProductDto> productDtos = [];

            foreach (Product product in products)
            {
                if (product != null)
                {
                    productDtos.Add(new ProductDto
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
                    });
                }
            }

            return productDtos;
        }

        return null;
    }

    /// <summary>
    /// Activates or deactivates a product.
    /// </summary>
    /// <param name="sku"></param>
    /// <returns></returns>
    public bool ToggleProductActiveStatus(string sku, string username) =>
        !string.IsNullOrEmpty(sku) && _permService.IsAuthorized(username,"ActivateProduct") && _productRepo.ToggleProductActiveStatus(sku);

    /// <summary>
    /// Edits a product's data.
    /// </summary>
    /// <param name="updatedProduct"></param>
    /// <returns></returns>
    public ProductDto? EditProduct(ProductDto updatedProduct, string username)
    {
        if (!string.IsNullOrEmpty(updatedProduct.SKU) && !string.IsNullOrEmpty(updatedProduct.Name) &&
           updatedProduct.Barcode != null && !string.IsNullOrEmpty(updatedProduct.Category) && updatedProduct.ImageUrl != null &&
           !string.IsNullOrEmpty(updatedProduct.Description) && updatedProduct.CostPrice >= 0.0 &&
           updatedProduct.CurrentStock >= 0.0 && updatedProduct.MinimumStockLevel >= 0.0 && updatedProduct.ReorderQuantity >= 0 &&
           updatedProduct.UnitPrice >= 0.0 && updatedProduct.UnitMeasurement != null  && updatedProduct.SupplierId >= 0 &&
           updatedProduct.MaximumStockLevel >= 0 && _permService.IsAuthorized(username,"EditProduct"))
            return _productRepo.EditProduct(updatedProduct) is Product p ? updatedProduct : null;
        return null;
    }
}