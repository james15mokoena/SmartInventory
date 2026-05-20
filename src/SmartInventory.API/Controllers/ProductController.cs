using System.Drawing.Imaging;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;
using SmartInventory.API.Services;

namespace SmartInventory.API.Controllers;

/// <summary>
/// Handles requests to the product management subsystem.
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class ProductController(ProductManagementService productService) : ControllerBase
{
    /// <summary>
    /// Enables interaction with the product management subsystem.
    /// </summary>
    private readonly ProductManagementService _productService = productService;

    /// <summary>
    /// Add a new a product.
    /// </summary>
    /// <param name="newProduct"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpPost("{username}")]
    public IActionResult AddProduct(ProductDto newProduct, string username) => _productService.AddProduct(newProduct,username) ?
                                                           CreatedAtAction(nameof(AddProduct), newProduct) :
                                                           BadRequest("Failed to add the product!");

    /// <summary>
    /// Fetches a product's details by SKU.
    /// </summary>
    /// <param name="sku"></param>
    /// <returns></returns>
    [HttpGet("{sku}/{username}")]
    public IActionResult ViewProductDetailsBySku(string sku, string username)
    {
        sku = ConverterService.FromBase64String(sku);

        if (_productService.GetProductBySku(sku, username) is ProductDto dto)
            return Ok(dto);

        return BadRequest("Failed to fetch product details!");
    }
        
    /// <summary>
    /// Fetches a product's details by name.
    /// </summary>
    /// <param name="sku"></param>
    /// <returns></returns>
    [HttpGet("{name}/{username}")]
    public IActionResult ViewProductDetailsByName(string name, string username) =>
        _productService.GetProductByName(name, username) is ProductDto dto ?
        Ok(dto) :
        BadRequest("Failed to fetch product details!");

    /// <summary>
    /// Fetches all active products.
    /// </summary>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult ViewActiveProducts(string username) => _productService.GetActiveProducts(username) is List<ProductDto> dtos ?
                                                           Ok(dtos) :
                                                           BadRequest("Failed to fetch active products!");

    /// <summary>
    /// Fetches all deactivated products.
    /// </summary>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult ViewDeactivatedProducts(string username) =>
        _productService.GetDeactivatedProducts(username) is List<ProductDto> dtos ?
        Ok(dtos) :
        BadRequest("Failed to fetch deactivated products!");

    /// <summary>
    /// Activates or deactivates a product.
    /// </summary>
    /// <param name="sku"></param>
    /// <returns></returns>
    [HttpPut("{sku}/{username}")]
    public IActionResult ActivateOrDeactivateProduct(string sku, string username) =>
        _productService.ToggleProductActiveStatus(ConverterService.FromBase64String(sku), username) ?
        Ok("Product status changed!") :
        BadRequest("Failed to change product's status!");

    /// <summary>
    /// Edits a product's data.
    /// </summary>
    /// <param name="updatedProduct"></param>
    /// <returns></returns>
    [HttpPut("{username}")]
    public IActionResult EditProduct(ProductDto updatedProduct, string username) =>
        _productService.EditProduct(updatedProduct, username) is ProductDto d ?
        Ok(d) : BadRequest("Failed to edit the product!");

    /// <summary>
    /// Fetches products that belong to the specified category.
    /// </summary>
    /// <param name="category"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{category}/{username}")]
    public IActionResult ViewProductsByCategory(string category, string username) => !string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(username) &&
        _productService.GetProductsByCategory(category, username) is List<ProductDto> products ? Ok(products) : BadRequest("Failed to fetch products!");
}