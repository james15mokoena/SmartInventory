namespace SmartInventory.API.Models;

/// <summary>
/// Defines the types of permissions that can be assigned to a role.
/// </summary>
public static class Permissions
{
    // ***** User Management permissions. ***** //

    public const string CreateUser = "CreateUser";

    public const string EditUser = "EditUser";

    public const string DeactivateUser = "DeactivateUser";

    public const string ViewUsers = "ViewUsers";


    // ***** Product Management permissions. ***** //

    public const string AddProduct = "AddProduct";

    public const string EditProduct = "EditProduct";

    public const string DeactivateProduct = "DeactivateProduct";

    public const string ViewProductDetails = "ViewProductDetails";

    public const string ViewProductHistory = "ViewProductHistory";


    // ***** Supplier Management permissions. ***** //

    public const string AddSupplier = "AddSupplier";

    public const string EditSupplier = "EditSupplier";

    public const string DeactivateSupplier = "DeactivateSupplier";

    public const string ViewSupplierDetails = "ViewSupplierDetails";


    // ***** Stock Management permissions ***** //

    public const string RecordIncomingStock = "RecordIncomingStock";

    public const string RecordOutgoingStock = "RecordOutgoingStock";

    public const string RecordAdjustment = "RecordAdjustment";

    public const string ViewStockLevels = "ViewStockLevels";


    // ***** Forecast Management permissions ***** //

    public const string ViewForecast = "ViewForecast";

    public const string ViewForecastAlerts = "ViewForecastAlerts";

    public const string RefreshForecasts = "RefreshForecasts";

    
    // ***** Procument Management permissions ***** //

    public const string CreatePurchaseOrder = "CreatePurchaseOrder";

    public const string EditPurchaseOrder = "EditPurchaseOrder";

    public const string ViewPurchaseOrder = "ViewPurchaseOrder";

    public const string ExportPurchaseOrderToPdf = "ExportPurchaseOrderToPdf";

    public const string UpdatePurchaseOrderStatus = "UpdatePurchaseOrderStatus";

    
    // ***** Reports Management permissions ***** //

    public const string ViewReports = "ViewReports";

    public const string ExportReports = "ExportReports";

    
    // ***** Notifications Management permissions ***** //

    public const string ViewLowStockAlerts = "ViewLowStockAlerts";

    public const string ViewStockoutPredictionAlerts = "ViewStockoutPredictionAlerts";

    public const string ViewSlowMovingAlerts = "ViewSlowMovingAlerts";
}