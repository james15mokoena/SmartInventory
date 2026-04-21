using Microsoft.AspNetCore.Mvc;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Services;

namespace SmartInventory.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class HomeController(HomeService homeService) : ControllerBase
{
    private readonly HomeService _homeServ = homeService;

    /// <summary>
    /// Gets the total revenue for the current month.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult ViewTotalRevenueCurrentMonth(string username) =>
        !string.IsNullOrEmpty(username) ?
        Ok(_homeServ.GetTotalRevenueCurrentMonth(username)) :
        BadRequest("Failed to get the total revenue!");

    /// <summary>
    /// Gets the total sales by categories.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult ViewTotalSalesByCategories(string username) =>
        !string.IsNullOrEmpty(username) &&
        _homeServ.GetTotalSalesByCategories(username) is List<TotalSalesByCategory> sales ?
        Ok(sales) :
        BadRequest("Failed to get the total sales by categories!");

    /// <summary>
    /// Gets the category with most sales.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult ViewCategoryWithMostSales(string username) =>
        !string.IsNullOrEmpty(username) && _homeServ.GetCategoryWithMostSales(username) is TotalSalesByCategory t ?
        Ok(t) : BadRequest("Failed to get the category with most sales!");

    /// <summary>
    /// Gets the category with least sales.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult ViewCategoryWithLeastSales(string username) =>
        !string.IsNullOrEmpty(username) && _homeServ.GetCategoryWithLeastSales(username) is TotalSalesByCategory t ?
        Ok(t) : BadRequest("Failed to get the category with least sales!");

    /// <summary>
    /// Gets products with the most sales in each category.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult ViewProductsWithMostSalesByCategory(string username) =>
        !string.IsNullOrEmpty(username) && _homeServ.GetProductsWithMostSalesByCategory(username) is
            List<TotalSalesByCategory> res ? Ok(res) : BadRequest("Failed to get products with most sales by category!");
    
    /// <summary>
    /// Gets products with the least sales in each category.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult ViewProductsWithLeastSalesByCategory(string username) =>
        !string.IsNullOrEmpty(username) && _homeServ.GetProductsWithLeastSalesByCategory(username) is
            List<TotalSalesByCategory> res ? Ok(res) : BadRequest("Failed to get products with least sales by category!");

}