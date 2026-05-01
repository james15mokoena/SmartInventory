using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Repositories;

namespace SmartInventory.API.Services;

public class HomeService(HomeRepository homeRepo, PermissionManagementService perm)
{
    private readonly HomeRepository _homeRepo = homeRepo;

    private readonly PermissionManagementService _permServ = perm;

    /// <summary>
    /// Query: <b>Which product categories contribute at most 50% of the total revenue for this month?</b>
    /// <param name="username"></param>
    /// </summary>
    /// <returns></returns>
    public List<CategorySales>? GetCategoriesContributingAtMost50PercentMonth(string username) =>
        !string.IsNullOrEmpty(username) && _permServ.IsAuthorized(username, "ViewReports") &&
        _homeRepo.GetCategoriesContributingAtMost50PercentMonth() is List<CategorySales> cs ?
        cs : null;

    /// <summary>
    /// Query: <b>Which product categories contribute at most 50% of the total revenue for this month?</b>
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public List<CategorySales>? GetCategoriesContributingMoreThan50PercentMonth(string username) =>
        !string.IsNullOrEmpty(username) && _permServ.IsAuthorized(username, "ViewReports") &&
        _homeRepo.GetCategoriesContributingMoreThan50PercentMonth() is List<CategorySales> cs ?
        cs : null;

    /// <summary>
    /// Answers: <b>Which product categories make up the top 5 most sales this month?</b>
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public List<CategorySales>? GetTopFiveMostSellingCategoriesThisMonth(string username) =>
        !string.IsNullOrEmpty(username) && _permServ.IsAuthorized(username, "ViewReports") &&
        _homeRepo.GetTopFiveMostSellingCategoriesThisMonth() is List<CategorySales> s ? s : null;

    /// <summary>
    /// Answers: <b>What is the total revenue for each month this year?</b>
    /// </summary>
    /// <returns></returns>
    public List<MonthlyRevenue>? GetMonthlyRevenues(string username) =>
        !string.IsNullOrEmpty(username) && _permServ.IsAuthorized(username, "ViewReports") &&
        _homeRepo.GetMonthlyRevenues() is List<MonthlyRevenue> revenues ? revenues : null;

    /// <summary>
    /// Gets the total revenue for the current month.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public double GetTotalRevenueCurrentMonth(string username) =>
        !string.IsNullOrEmpty(username) && _permServ.IsAuthorized(username, "ViewReports") ?
        _homeRepo.GetTotalRevenueCurrentMonth() : 0;

    /// <summary>
    /// Gets the total sales for each product category.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public List<TotalSalesByCategory>? GetTotalSalesByCategories(string username) =>
        !string.IsNullOrEmpty(username) && _permServ.IsAuthorized(username, "ViewReports") ?
        _homeRepo.GetTotalSalesByCategories() :
        null;

    /// <summary>
    /// Answers: <b>Which product category has the most sales in the current month?</b>
    /// </summary>
    /// <returns></returns>
    public TotalSalesByCategory? GetCategoryWithMostSales(string username) =>
        !string.IsNullOrEmpty(username) &&  _permServ.IsAuthorized(username,"ViewReports") &&
        _homeRepo.GetCategoryWithMostSales() is TotalSalesByCategory t ?
        t : null;

    /// <summary>
    /// Answers: <b>Which product category has the least sales in the current month?</b>
    /// </summary>
    /// <returns></returns>
    public TotalSalesByCategory? GetCategoryWithLeastSales(string username) =>
        !string.IsNullOrEmpty(username) && _permServ.IsAuthorized(username,"ViewReports") &&
        _homeRepo.GetCategoryWithLeastSales() is TotalSalesByCategory t
        ? t : null;

    /// <summary>
    /// Answers: <b>Which product has the most sales in each category?</b>
    /// </summary>
    /// <returns></returns>
    public List<TotalSalesByCategory>? GetProductsWithMostSalesByCategory(string username) =>
        !string.IsNullOrEmpty(username) && _permServ.IsAuthorized(username, "ViewReports") &&
        _homeRepo.GetProductsWithMostSalesByCategory() is List<TotalSalesByCategory> ts ?
        ts : null;

    /// <summary>
    /// Answers: <b>Which product has the least sales in each category?</b>
    /// </summary>
    /// <returns></returns>
    public List<TotalSalesByCategory>? GetProductsWithLeastSalesByCategory(string username) =>
        !string.IsNullOrEmpty(username) && _permServ.IsAuthorized(username, "ViewReports") &&
        _homeRepo.GetProductsWithLeastSalesByCategory() is List<TotalSalesByCategory> ts ?
        ts : null;
}