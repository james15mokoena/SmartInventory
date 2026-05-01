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
    /// Gets categories that contribute at most 50% towards the revenue this month.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult GetCategoriesContributingAtMost50PercentMonth(string username) =>
        !string.IsNullOrEmpty(username) && _homeServ.GetCategoriesContributingAtMost50PercentMonth(username)
        is List<CategorySales> res ? Ok(res) : BadRequest("Failed to get the data!");

    /// <summary>
    /// Gets categories that contribute more than 50% towards the revenue this month.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult GetCategoriesContributingMoreThan50PercentMonth(string username) =>
        !string.IsNullOrEmpty(username) && _homeServ.GetCategoriesContributingMoreThan50PercentMonth(username)
        is List<CategorySales> res ? Ok(res) : BadRequest("Failed to get the data!");

    /// <summary>
    /// Gets the top 5 most selling categories.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult GetTopFiveMostSellingCategoriesThisMonth(string username) =>
        !string.IsNullOrEmpty(username) && _homeServ.GetTopFiveMostSellingCategoriesThisMonth(username) is List<CategorySales> s ? Ok(s) : BadRequest("Failed to get the top 5 most selling products.");

    /// <summary>
    /// Gets the monthly revenues in the current year.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public IActionResult ViewMonthlyRevenues(string username) =>
        !string.IsNullOrEmpty(username) ? Ok(_homeServ.GetMonthlyRevenues(username)) :
        BadRequest("Failed to get monthly revenues!");

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