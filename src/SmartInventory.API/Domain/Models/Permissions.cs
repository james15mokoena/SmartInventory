namespace SmartInventory.API.Domain.Models;

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

    public const string ActivateProduct = "ActivateProduct";

    public const string ViewProductDetails = "ViewProductDetails";

    public const string ViewProductHistory = "ViewProductHistory";

    public const string ViewProducts = "ViewProducts";


    // ***** Supplier Management permissions. ***** //

    public const string AddSupplier = "AddSupplier";

    public const string EditSupplier = "EditSupplier";

    public const string DeactivateSupplier = "DeactivateSupplier";

    public const string ViewSupplierDetails = "ViewSupplierDetails";


    // ***** Stock Management permissions ***** //

    public const string RecordIncomingStock = "RecordIncomingStock";

    public const string RecordOutgoingStock = "RecordOutgoingStock";

    public const string RecordAdjustment = "RecordAdjustment";

    public const string ViewStockLevels = "ViewStockReports";

    public const string AddTransactionReason = "AddTransactionReason";

    public const string ViewTransactionReasons = "ViewTransactionReasons";

    public const string ViewStockTransactions = "ViewStockTransactions";

    public const string UpdateTransactionReason = "UpdateTransactionReason";

    // ***** Forecast Management permissions ***** //

    public const string ViewForecast = "ViewForecast";

    public const string ViewForecastAlerts = "ReceiveForecastAlerts";

    //public const string RefreshForecasts = "RefreshForecasts";


    // ***** Procument Management permissions ***** //

    public const string CreatePurchaseOrder = "CreatePurchaseOrder";

    public const string EditPurchaseOrder = "EditPurchaseOrder";

    public const string ViewPurchaseOrder = "ViewPurchaseOrder";

    public const string ExportPurchaseOrderToPdf = "ExportPurchaseOrderToPdf";

    public const string UpdatePurchaseOrderStatus = "UpdatePurchaseOrderStatus";

    public const string GenerateQuotation = "GenerateQuotation";

    public const string GenerateOrder = "GenerateOrder";


    // ***** Reports Management permissions ***** //

    public const string ViewReports = "ViewReports";

    public const string ExportReports = "ExportReports";


    // ***** Sales Management permissions ***** //
    public const string GenerateRequisitionForm = "GenerateRequisitionForm";

    public const string GenerateTaxInvoice = "GenerateTaxInvoice";

    // ***** Permissions Management permissions ***** //

    public const string AssignPermission = "AssignPermission";

    public const string UnassignPermission = "UnassignPermission";

    public const string All = "All";

    public const string AddRole = "AddRole";

    public const string UpdateRole = "UpdateRole";

    public const string UpdatePermission = "UpdatePermission";

    public const string ViewPermission = "ViewPermissions";

    public const string ViewRoles = "ViewRoles";
}