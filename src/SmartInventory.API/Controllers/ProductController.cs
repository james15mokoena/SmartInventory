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
    /// <returns></returns>
    [HttpPost]
    public IActionResult AddProduct(ProductDto newProduct) => _productService.AddProduct(newProduct) ?
                                                           CreatedAtAction(nameof(AddProduct), newProduct) :
                                                           BadRequest("Failed to add the product!");

    /// <summary>
    /// Fetches a product's details.
    /// </summary>
    /// <param name="sku"></param>
    /// <returns></returns>
    [HttpGet("{sku}")]
    public IActionResult ViewProductDetails(string sku) => _productService.GetProductBySku(sku) is ProductDto dto ?
                                                           Ok(dto) :
                                                           BadRequest("Failed to fetch product details!");
}