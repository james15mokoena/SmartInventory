using System.Text;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;
using SmartInventory.API.Repositories;

namespace SmartInventory.API.Services;

/// <summary>
/// 
/// </summary>
/// <param name="stockRepo"></param>
public class StockManagementService(StockManagementRepository stockRepo, UserManagementService userService, PermissionManagementService permServ)
{
    /// <summary>
    /// Used to interact with the database.
    /// </summary>
    private readonly StockManagementRepository _stockRepo = stockRepo;

    /// <summary>
    /// Used to interact with the user management service.
    /// </summary>
    private readonly UserManagementService _userService = userService;

    /// <summary>
    /// Used to interact with the permission management service.
    /// </summary>
    private readonly PermissionManagementService _permService = permServ;

    /// <summary>
    /// Used to record a stock transaction that adds stocks.
    /// </summary>
    /// <param name="sku">A product's stock-keeping unit number.</param>
    /// <param name="quantity">The quantity to be added to product.</param>
    /// <param name="username">An identifier for the user who initiated the transaction.</param>
    /// <param name="reason">The reason for which the transaction was initiated.</param>
    /// <param name="isNewProduct">Indicates whether a new product is added.</param>
    /// <returns></returns>
    public bool RecordIncomingStock(string sku, int quantity, string username, string reason, bool isNewProduct) =>
        !string.IsNullOrEmpty(sku) && quantity > 0 && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(reason) &&
        _permService.IsAuthorized(username, "RecordIncomingStock") && _userService.GetStaffMember(username) is Staff staff &&
        _stockRepo.RecordIncomingStock(sku, quantity, staff.Id, reason, isNewProduct);

    /// <summary>
    /// Used to deduct the specified quantity from the stock quantity.
    /// </summary>
    /// <param name="sku"></param>
    /// <param name="quantity"></param>
    /// <param name="username"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public bool RecordOutgoingStock(string sku, int quantity, string username, string reason) => 
    !string.IsNullOrEmpty(sku) && quantity > 0 && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(reason) &&
        _permService.IsAuthorized(username, "RecordOutgoingStock") && _userService.GetStaffMember(username) is Staff staff &&
        _stockRepo.RecordOutgoingStock(sku, quantity, staff.Id, reason);

    /// <summary>
    /// Used to add a new transaction reason.
    /// </summary>
    /// <param name="reasonType"></param>
    /// <returns></returns>
    public bool AddTransactionReason(ReasonType reasonType) => !string.IsNullOrEmpty(reasonType.Reason) && _stockRepo.AddTransactionReason(reasonType);

    /// <summary>
    /// Used to delete a transaction reason.
    /// </summary>
    /// <param name="reasonTypeId"></param>
    /// <returns></returns>
    public bool ToggleTransactionReasonStatus(int reasonTypeId) => reasonTypeId >= 0 && _stockRepo.ToggleTransactionReasonStatus(reasonTypeId);

    /// <summary>
    /// Used to get all transaction reasons.
    /// </summary>
    /// <returns></returns>
    public List<ReasonType?>? GetTransactionReasons() => _stockRepo.GetTransactionReasons();

    /// <summary>
    /// Used to fetch all stock transactions.
    /// </summary>
    /// <returns></returns>
    public List<StockTransactionDto>? GetStockTransactions()
    {
        List<StockTransaction>? stockTransactions = _stockRepo.GetStockTransactions();

        if (stockTransactions != null && stockTransactions.Count > 0)
        {
            List<StockTransactionDto> stockTransactionDtos = [];
            foreach (StockTransaction stockTransaction in stockTransactions)
                stockTransactionDtos.Add(ToStockTransactionDto(stockTransaction));

            return stockTransactionDtos;
        }
        return null;
    }

     /// <summary>
    /// Used to fetch a product's stock transactions.
    /// </summary>
    /// <returns></returns>
    public List<StockTransactionDto>? GetStockTransactionsBySku(string sku)
    {
        List<StockTransaction>? stockTransactions = _stockRepo.GetStockTransactionsBySku(sku);

        if (stockTransactions != null && stockTransactions.Count > 0)
        {
            List<StockTransactionDto> stockTransactionDtos = [];
            foreach (StockTransaction stockTransaction in stockTransactions)
                stockTransactionDtos.Add(ToStockTransactionDto(stockTransaction));

            return stockTransactionDtos;
        }
        return null;
    }

    /// <summary>
    /// Used to adjust a product's stock.
    /// </summary>
    /// <param name="sku"></param>
    /// <param name="quantity"></param>
    /// <param name="username"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public bool RecordStockAdjustment(string sku, int quantity, string username, string reason)
    {
        // FIX: Username must be used
        if (!string.IsNullOrEmpty(sku) && quantity > 0 && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(reason))
            return _stockRepo.RecordStockAdjustment(sku, quantity, 0, reason);
        return false;
    }

    /// <summary>
    /// Generates the stock report.
    /// </summary>
    /// <param name="company"></param>
    /// <param name="signature"></param>
    /// <returns></returns>
    public StockReport? GetStockReport(string company, string signature)
    {

        if (!string.IsNullOrEmpty(company) && !string.IsNullOrEmpty(signature) && _permService.IsAuthorized(signature, "ViewStockReports"))
        {

            // the name of the person generating the stock report.
            string fullName = $"{ _userService.GetStaffMember(signature)!.FirstName} {_userService.GetStaffMember(signature)!.LastName}";

            if (_stockRepo.GetStockReport(company, signature) is StockReport stockReport)
            {
                // set signature to the name of the person generating the report.
                stockReport.Signature = fullName;

                // generate an HTML document with this information.
                StringBuilder builder = new();

                // used to build the stock report table.
                StringBuilder tableBuilder = new();

                // build the stock table
                foreach (StockReportItem item in stockReport.Items)
                {
                    tableBuilder.Append(
                        $@"
                            <tr>
                                <td style='border-style:solid;border-width:1px;border-color:black;border-left-style:none;'>
                                    {item.Name}
                                </td>
                                <td style='border-style:solid;border-width:1px;border-color:black;'>
                                    {item.Code}
                                </td>
                                <td style='border-style:solid;border-width:1px;border-color:black;'>
                                    {item.StockLevel}
                                </td>
                                <td style='border-style:solid;border-width:1px;border-color:black;'>
                                    {item.ReorderLevel}
                                </td>
                                <td style='border-style:solid;border-width:1px;border-color:black;'>
                                    {item.MaximumLevel}
                                </td>
                                <td style='border-style:solid;border-width:1px;border-color:black;border-right-style:none;'>
                                    {item.IsReorder}
                                </td>
                            </tr>
                        "
                    );
                }

                // stores the body of the document.
                string body =
                    $@"
                        <div class='container' style='border-style:solid;border-color:black;'>

                            <h1 style='text-align:center;border-style;padding:2px;border-bottom-style:solid;border-color:black;'>
                                {stockReport.CompanyName}
                            </h1>

                            <h3 style='text-align:center;border-bottom-style:solid;border-color:black;padding:2px;margin-bottom:0;'>
                                Stock (Inventory) Report
                            </h3>
                            
                            <div class='stock-table' style='margin-bottom:7px;margin-top:0;padding-top:0;border-top-style:none;font-size:16px;
                                width:100%;overflow-x:auto;overflow-y:auto;'>
                                <table style='border-style:solid;border-width:1px;border-color:black;border-collapse:collapse;text-align:center;width:100%;
                                    border-left-style:none;border-right-style:none;padding-top:0;margin-top:0;'>
                                    <thead style='font-weight:bold;background-color:#f5f5dc;'>
                                        <tr>
                                            <th style='border-style:solid;border-width:1px;border-color:black;border-left-style:none;'> Item </th>
                                            <th style='border-style:solid;border-width:1px;border-color:black;'> Code </th>
                                            <th style='border-style:solid;border-width:1px;border-color:black;'> Stock Level <br /> (units) </th>
                                            <th style='border-style:solid;border-width:1px;border-color:black;'> Reorder Level <br /> (units) </th>
                                            <th style='border-style:solid;border-width:1px;border-color:black;border-right-style:none;'> Maximum Level <br /> (units) </th>
                                            <th style='border-style:solid;border-width:1px;border-color:black;'> Reorder? <br > (Yes/No) </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {tableBuilder}
                                    </tbody>
                                </table>
                            </div>

                            <div class='signature-date'>
                                
                                <span id='sign' style='font-size:22px;text-align:start;margin-left:5px;margin-right:50%;'>
                                    <span style='font-weight:bold;margin-right:4px;margin-left:5px;'>
                                        Signature:
                                    </span> 
                                    <span style='font-style:italic;'>
                                        {stockReport.Signature}
                                    </span>
                                </span>

                                <span id='dat' style='font-size:22px;'>
                                    <span style='font-weight:bold;margin-right:4px;'>
                                        Date:
                                    </span> 
                                    <span style='font-style:italic;'>
                                        {stockReport.DateGenerated}
                                    </span>
                                </span>

                            </div>

                        </div>
                    ";

                string trHoverStyle = "tbody tr:hover{background-color:#87cefa;}";
                string respBody =
                    @"
                        @media (max-width: 1194px) {

                            #dat{
                                display: block;
                                margin-top: 7px;                                
                                text-align: center;
                            }
                        }
                    ";

                string html =
                    $@"
                        <!DOCTYPE html>
                        <html>
                            <head>
                                <title> Stock (Inventory) Report </title>
                                <style type='text/css'>
                                    {trHoverStyle}
                                    {respBody}
                                </style>
                            </head>

                            <body>
                                ${body}
                            </body>
                        </html>
                    ";

                builder.Append(html);
                builder.Replace('$', ' ');

                // build the destination folder and file
                string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string filePath = Path.Combine(path, "Downloads", "StockReport.html");

                using StreamWriter writer = new(filePath, false, Encoding.UTF8);
                writer.Write(builder.ToString());
                return stockReport;
            }
        }
        return null;
    }
        
    /// <summary>
    /// Converts a StockTransaction object to StockTransactionDto object.
    /// </summary>
    /// <param name="stockTransaction"></param>
    /// <returns></returns>
    private StockTransactionDto ToStockTransactionDto(StockTransaction stockTransaction)
    {
        return new StockTransactionDto
        {
            TransactionId = stockTransaction.TransactionId
            ,
            UserId = stockTransaction.UserId
            ,
            ProductId = stockTransaction.ProductId
            ,
            NewStock = stockTransaction.NewStock
            ,
            PreviousStock = stockTransaction.PreviousStock
            ,
            QuantityChange = stockTransaction.QuantityChange
            ,
            Date = stockTransaction.Date
            ,
            ReasonTypeId = stockTransaction.ReasonTypeId
            ,
            Reason = _stockRepo.GetTransactionReason(stockTransaction.ReasonTypeId)
        };
    }
}