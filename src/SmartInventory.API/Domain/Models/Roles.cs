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
    public const string Manager = "Manager";

    /// <summary>
    /// Can manually create stock transactions, in addition to automatic creation.
    /// </summary>
    public const string InventoryManager = "StockManager";

    /// <summary>
    /// Creates and manages purchase orders.
    /// </summary>
    public const string ProcumentOfficer = "ProcurementManager";

    /// <summary>
    /// Sells items.
    /// </summary>
    public const string SalesUser = "SalesManager";

    /// <summary>
    /// Can view but not modify.
    /// </summary>
    public const string Viewer = "Viewer";
}