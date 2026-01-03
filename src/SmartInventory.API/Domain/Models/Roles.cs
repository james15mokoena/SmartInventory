namespace SmartInventory.API.Domain.Models;

/// <summary>
/// Defines roles that exist in the system
/// </summary>
public static class Roles
{
    /// <summary>
    /// Manages users, and permissions.
    /// </summary>
    public const string Admin = "Admin";

    /// <summary>
    /// Approves stock adjustments and purchase orders.
    /// </summary>
    public const string GeneralManager = "General Manager";

    /// <summary>
    /// Can manually create stock transactions, in addition to automatic creation.
    /// </summary>
    public const string StockManager = "Stock Manager";

    /// <summary>
    /// Creates and manages purchase orders.
    /// </summary>
    public const string ProcumentManager = "Procurement Manager";

    /// <summary>
    /// Performs sales related functions on the system.
    /// </summary>
    public const string SalesManager = "Sales Manager";

    /// <summary>
    /// Can view but not modify.
    /// </summary>
    public const string GeneralUser = "General User";
}