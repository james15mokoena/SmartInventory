using System.Text;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Repositories;

namespace SmartInventory.API.Services;

/// <summary>
/// Defines the functionality for providing services performed by the purchase
/// department.
/// </summary>
public class ProcurementManagementService(ProcurementManagementRepository procRepo, PermissionManagementService premServ,
    UserManagementService userServ, SupplierManagementService suppServ)
{
    /// <summary>
    /// Used to interact with the permission subsystem.
    /// </summary>
    private readonly PermissionManagementService _permServ = premServ;

    /// <summary>
    /// Used to interact with the user subsystem.
    /// </summary>
    private readonly UserManagementService _userServ = userServ;

    /// <summary>
    /// Used to interact with the database.
    /// </summary>
    private readonly ProcurementManagementRepository _procRepo = procRepo;

    /// <summary>
    /// Used to interact with the supplier subsystem.
    /// </summary>
    private readonly SupplierManagementService _suppServ = suppServ;

    /// <summary>
    /// Generates a quotation.
    /// </summary>
    /// <param name="quote"></param>
    /// <returns></returns>
    public QuotationDto? GenerateQuotation(QuotationDto quote)
    {
        if (!string.IsNullOrEmpty(quote.QuotedBy) && !string.IsNullOrEmpty(quote.SuppliedTo) && quote.DateGenerated != default &&
            quote.QuotationItems.Count > 0 && !string.IsNullOrEmpty(quote.Company) && !string.IsNullOrEmpty(quote.HouseNoAndStreetName) &&
            !string.IsNullOrEmpty(quote.TelephoneNo) && !string.IsNullOrEmpty(quote.TownOrSuburb) && !string.IsNullOrEmpty(quote.PoBoxAddress) &&
            !string.IsNullOrEmpty(quote.PostOfficeLocation) && _procRepo.AddQuotation(quote) is int quoteNo && quoteNo >= 0 &&
            _permServ.IsAuthorized(quote.QuotedBy, "GenerateQuotation"))
        {
            // set the name of the person for QuotedBy field
            quote.QuotedBy = $"{_userServ.GetStaffMember(quote.QuotedBy!)!.FirstName}  {_userServ.GetStaffMember(quote.QuotedBy!)!.LastName}";
            // update quotation ID to the correct one
            quote.Id = quoteNo;

            // used to build the HTML content
            StringBuilder htmlBuilder = new();

            // stores the vat registration number
            //string? vatRegNo = quote.VatRegNo;

            StringBuilder tableBuilder = new();

            string trHoverStyles = "tbody tr:hover{background-color:#dda0dd;}";

            // stores the total price
            double totalPrice = 0;

            foreach (QuotationItemDto item in quote.QuotationItems)
            {
                totalPrice += item.TotalPrice;

                tableBuilder.Append(
                    $@"
                        <tr>
                            <td style='font-size:22px;border-width:2px;border-style:solid;border-color:black;text-align:center;'>
                                {item.Code}
                            </td>
                            <td style='font-size:22px;border-width:2px;border-style:solid;border-color:black;text-align:center;'>
                                {item.Description}
                            </td>
                            <td style='font-size:22px;border-width:2px;border-style:solid;border-color:black;text-align:center;'>
                                {item.Quantity}
                            </td>
                            <td style='font-size:22px;border-width:2px;border-style:solid;border-color:black;text-align:center;'>
                                {item.UnitPrice}
                            </td>
                            <td style='font-size:22px;border-width:2px;border-style:solid;border-color:black;text-align:center;'>
                                {item.TotalPrice}
                            </td>
                        </tr>
                    "
                );
            }

            // add the row that contains the total
            tableBuilder.Append(
                $@"
                    <tr style='padding:25px;'>
                        <td colspan='4' style='padding-right:12px;font-weight:bold;font-size:28px;border-width:2px;border-style:solid;border-color:black;text-align:end;'>
                            TOTAL:
                        </td>
                        <td colspan='1' style='font-size:28px;font-weight:bold;border-width:2px;border-style:solid;border-color:black;text-align:center;'>
                            {totalPrice}
                        </td>
                    </tr>
                "
            );

            string body =
                $@"
                    <div class='main-cont' style='border-style:solid;border-width:2px;border-color:black;overflow-x:auto;overflow-y:auto;'>

                        <h1 style='text-align:center;margin-top:3px;'> {quote.Company} </h1>                        
                        
                        {(!string.IsNullOrEmpty(quote.VatRegNo) ?
                            "<p style='text-align:center;font-size:19px;'> Vat Registration No. " + quote.VatRegNo +
                            "</p>" :
                            ""
                        )}

                        <div class='addr-cont' style='display:flex;flex-direction:row;flex-wrap:wrap;justify-content:space-evenly;'>

                            <div class='phys-addr'>
                                <p style='font-size:19px;font-weight:bold;'>{quote.HouseNoAndStreetName} </p>
                                <p style='font-size:19px;font-weight:bold;'>{quote.TownOrSuburb} </p>
                                <p style='font-size:19px;font-weight:bold;'> Tel/Phone: {quote.TelephoneNo} </p>
                            </div>

                            {(!string.IsNullOrEmpty(quote.CompanyLogo) ?
                                "<div class='logo'>" +
                                    "<img src='' alt='Company logo' />" :
                                ""
                            )}

                            <div class='postal-addr'>
                                <p style='font-size:19px;font-weight:bold;'>{quote.PoBoxAddress} </p>
                                <p style='font-size:19px;font-weight:bold;'>{quote.PostOfficeLocation} </p>
                                {(!string.IsNullOrEmpty(quote.FaxNo) ?
                                    "<p style='font-size:19px;font-weight:bold;'> Fax: " + quote.FaxNo +
                                    "</p>" :
                                    ""
                                )}
                            </div>
                            
                        </div>

                        <div class='doc-name' style='border-style:solid;border-color:black;border-width:2px;border-left-style:none;
                            border-right-style:none;'>
                            <h2 style='text-align:center;padding:5px;'> QUOTATION </h2>
                        </div>

                        <div style='display:flex;flex-direction:row;flex-wrap:wrap;justify-content:space-evenly;padding:5px;border-bottom-style:solid;
                            border-bottom-color:black;border-bottom-width:2px;'>

                            <div>
                                <span style='font-weight:bold;font-size:22px;margin-right:5px;margin-left:3px;'>
                                    Number:
                                </span>

                                <span style='font-size:22px;'>
                                    Q.{quote.Id}
                                </span>
                            </div>

                            <div style='font-size:22px;'> | </div>

                            <div>
                                <span style='font-weight:bold;font-size:22px;margin-right:5px;margin-left:3px;'>
                                    Date:
                                </span>

                                <span style='font-size:22px;'>
                                    {quote.DateGenerated}
                                </span>
                            </div>
                        </div>

                        <div style='padding:26px;border-bottom-style:solid;border-bottom-width:2px;border-bottom-color:black;'>
                            <span style='margin-left:20px;font-size:22px;font-weight:bold;margin-right:20px;'>
                                Supplied to:
                            </span>

                            <span style='padding-left:10px;padding-top:10px;padding-bottom:10px;font-style:italic;font-size:22px;'>
                                {quote.SuppliedTo}
                            </span>
                        </div>

                        <div class='quote-table' style='overflow-x:auto;overflow-y:auto;border-style:none;border-width:0;'>
                            <table style='border-collapse:collapse;border-color:black;overflow-x:auto;overflow-y:auto;width:100%;border-style:none;'>
                                <thead style='background-color:#b0e0e6;border-top-style:none;border-left-style:none;border-right-style:none;'>
                                    <tr>
                                        <th style='font-weight:bold;font-size:22px;border-width:2px;border-style:solid;border-color:black;text-align:center;'>
                                            Code
                                        </th>
                                        <th style='font-weight:bold;font-size:22px;border-width:2px;border-style:solid;border-color:black;text-align:center;'>
                                            Description
                                        </th>
                                        <th style='font-weight:bold;font-size:22px;border-width:2px;border-style:solid;border-color:black;text-align:center;'>
                                            Quantity
                                        </th>
                                        <th style='font-weight:bold;font-size:22px;border-width:2px;border-style:solid;border-color:black;text-align:center;'>
                                            Unit Price (R)
                                        </th>
                                        <th style='font-weight:bold;font-size:22px;border-width:2px;border-style:solid;border-color:black;text-align:center;'>
                                            Total Price (R)
                                        </th>
                                    </tr>
                                </thead>
                                <tbody style='border-left-style:none;border-right-style:none;'>
                                    {tableBuilder}
                                </tbody>
                            </table>
                        </div>

                        <div style='padding:17px;border-top-style:none;border-left-style:none;border-right-style:none;'>

                            <p style='font-weight:bold;font-size:23px;margin-botttom:7px;'>
                                {(!string.IsNullOrEmpty(quote.VatRegNo) ?
                                    "&#183 Prices include VAT" :
                                    "&#183 Prices don't include VAT"
                                )}
                            </p>

                            <p style='font-weight:bold;font-size:23px;margin-botttom:10px;'>
                                &#183 Quotation applicable for 30 days
                            </p>

                            <div style='display:flex;flex-direction:row;justify-content:space-evenly;'>
                                
                                <div>
                                    <span style='font-weight:bold;font-size:23px;margin-right:6px;'> Quoted by: </span>
                                    <span style='font-style:italic;font-size:23px;text-decoration:underline;'> {quote.QuotedBy} </span>
                                </div>

                                <div>
                                    <span style='font-weight:bold;font-size:23px;margin-right:6px;'> Signature: </span>
                                    <span style='font-style:italic;font-size:23px;text-decoration:underline;'> {quote.Signature} </span>
                                </div>

                            </div>

                        </div>

                    </div>
                ";

            string html =
                $@"
                    <!DOCTYPE html>
                    <html>
                        <head>
                            <title>Quotation</title>
                            <style type='text/css'>
                                {trHoverStyles}
                            </style>
                        </head>

                        <body>
                            {body}
                        </body>

                    </html>
                ";

            htmlBuilder.Append(html);

            // generate an HTML file.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = Path.Combine(path, "Downloads", "Quotation.html");

            using StreamWriter htmlWriter = new(filePath, false, Encoding.UTF8);
            htmlWriter.Write(htmlBuilder);
            return quote;
        }

        return null;
    }

    /// <summary>
    /// Generate an order.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public OrderDto? GenerateOrder(OrderDto dto)
    {
        if(IsStringValid(dto.CompanyName) && IsStringValid(dto.CompanyPoBoxAddress) && IsStringValid(dto.CompanyPoBoxTownAndCode) &&
            IsStringValid(dto.CompanyStreetNoAndStreetName) && IsStringValid(dto.CompanyTelephoneNo) && IsStringValid(dto.OrderedBy) &&
            IsStringValid(dto.Signature) && IsStringValid(dto.SupplierHouseNoAndStreetName) && IsStringValid(dto.SupplierName) &&
            IsStringValid(dto.SupplierTownAndCode) && dto.FaxNo != null && dto.VatRegNo != null && dto.DateGenerated != default &&
            dto.DeliveryDate != default && dto.RequisitionDate != default && dto.RequisitionNo >= 0 && dto.QuotationNo >= 0 &&
            dto.OrderItems.Count > 0 && _suppServ.SupplierExists(dto.SupplierName!) && _permServ.IsAuthorized(dto.OrderedBy!, "GenerateOrder") &&
            _procRepo.AddOrder(dto) is int orderId && orderId >= 0)
        {
            // update order id, to the generated ID.
            dto.Id = orderId;

            // update ordered by to the name of the person who placed the order.
            dto.OrderedBy = $"{_userServ.GetStaffMember(dto.OrderedBy!)!.FirstName} {_userServ.GetStaffMember(dto.OrderedBy!)!.LastName}";

            // used to build the HTML UI.
            StringBuilder htmlBuilder = new();

            // used to build the order table
            StringBuilder tableBuilder = new();

            // styles for table data.
            string tdStyles = "border-style:solid;border-width:2px;border-color:black;";

            // stores the total price to be displayed.
            double totalPrice = 0;

            foreach (OrderItemDto item in dto.OrderItems)
            {
                string tr =
                    $@"
                        <tr>
                            <td style='{tdStyles}text-align:center;border-left-style:none;'>
                                <span style='font-size:22px;'>{item.Code}</span>
                            </td>
                            <td style='{tdStyles}text-align:center;'>
                                <span style='font-size:22px;'>{item.Description}</span>
                            </td>
                            <td style='{tdStyles}text-align:center;'>
                                <span style='font-size:22px;'>{item.Quantity}</span>
                            </td>
                            <td style='{tdStyles}text-align:center;'>
                                <span style='font-size:22px;'>{item.UnitPrice}</span>
                            </td>
                            <td style='{tdStyles}text-align:center;border-right-style:none;'>
                                <span style='font-size:22px;'>{item.TotalAmount}</span>
                            </td>
                        </tr>
                    ";

                totalPrice += item.TotalAmount;

                tableBuilder.Append(tr);
            }

            tableBuilder.Append(
                $@"
                    <tr>
                        <td colspan='4' style='{tdStyles}text-align:end;border-left-style:none;'>
                            <span style='font-weight:bold;font-size:25px;padding-right:6px;'> TOTAL INCLUSIVE 14% VAT</span>
                        </td>
                        <td colspan='1' style='{tdStyles}text-align:center;border-right-style:none;'>
                            <span style='font-weight:bold;font-size:25px;text-align:center;'> {totalPrice}</span>
                        </td>
                    </tr>
                "
            );

            string body =
                $@"
                    <div style='border-style:solid;border-color:black;border-width:2px;'>
                        <h1 style='font-weight:bold;text-align:center;'>Order</h1>

                        <div style='text-align:center;border-color:black;border-style:solid;border-width:2px;border-left-style:none;
                            border-right-style:none;border-top-style:none;'>
                            <h1 style='font-weight:bold;font-size:50px;'>{dto.CompanyName} </h1>
                            {(
                                !string.IsNullOrEmpty(dto.VatRegNo) ?
                                "<h2 style='text-align:center;margin-top:7px;'> Vat registration number: " + dto.VatRegNo + "</h2>":
                                ""
                            )}
                        </div>

                        <div style='border-top-style:none;border-right-style:none;border-left-style:none;border-width:2px;border-color:black;
                            overflow-x:auto;overflow-y:auto;'>

                            <h2 style='text-align:center;font-weight:bold;font-size:22px;margin-bottom:5px;'>
                                {dto.CompanyStreetNoAndStreetName}, {dto.CompanyPoBoxTownAndCode!.Split(' ')[0]} - 
                                {dto.CompanyPoBoxAddress}, {dto.CompanyPoBoxTownAndCode}
                            </h2>

                            <h2 style='text-align:center;font-weight:bold;font-size:22px;'>
                                Tel: {dto.CompanyTelephoneNo} <span style='margin-right:5px;'></span> Fax: {dto.FaxNo ?? ""}
                            </h2>

                            <table style='border-collapse:collapse;border-width:2px;border-color:black;width:100%;'>
                                <tbody>
                                    <tr'>
                                        <td style='{tdStyles}border-left-style:none;background-color:#ffe4e1;'>
                                            <span style='font-weight:bold;font-size:22px;'> Order No:</span>
                                        </td>
                                        <td style='{tdStyles}text-align:center;'>
                                            <span style='font-size:22px;'> {dto.Id} </span>
                                        </td>
                                        <td style='{tdStyles}background-color:#ffe4e1;'>
                                            <span style='font-weight:bold;font-size:22px;'> Date:</span>
                                        </td>
                                        <td style='{tdStyles}text-align:center;border-right-style:none;'>
                                            <span style='font-size:22px;'> {dto.DateGenerated} </span>
                                        </td>
                                    </tr>
                                    <tr style='border-left-style:none;border-right-style:none;'>
                                        <td style='{tdStyles}border-left-style:none;background-color:#ffe4e1;'>
                                            <span style='font-weight:bold;font-size:22px;'> Requisition No:</span>
                                        </td>
                                        <td style='{tdStyles}text-align:center;'>
                                            <span style='font-size:22px;'> {dto.RequisitionNo} </span>
                                        </td>
                                        <td style='{tdStyles}background-color:#ffe4e1;'>
                                            <span style='font-weight:bold;font-size:22px;'> Quote No:</span>
                                        </td>
                                        <td style='{tdStyles}text-align:center;border-right-style:none;'>
                                            <span style='font-size:22px;'> {dto.QuotationNo} </span>
                                        </td>
                                    </tr>
                                    <tr style='border-left-style:none;border-right-style:none;'>
                                        <td style='{tdStyles}border-left-style:none;background-color:#ffe4e1;'>
                                            <span style='font-weight:bold;font-size:22px;'> Requisition date:</span>
                                        </td>
                                        <td style='{tdStyles}text-align:center;'>
                                            <span style='font-size:22px;'> {dto.RequisitionDate} </span>
                                        </td>
                                        <td style='{tdStyles}background-color:#ffe4e1;'>
                                            <span style='font-weight:bold;font-size:22px;'> Delivery on/before:</span>
                                        </td>
                                        <td style='{tdStyles}text-align:center;border-right-style:none;'>
                                            <span style='font-size:22px;'> {dto.DeliveryDate} </span>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>

                            <div style='display:flex;flex-direction:row;flex-wrap:wrap;margin-top:18px;margin-bottom:18px;'>
                                
                                <span style='font-size:25px;margin-left:6px;margin-right:35px;'>Supplier:</span>

                                <div style='display:flex;flex-direction:column;flex-wrap:wrap;'>
                                    <span style='font-weight:bold;font-size:25px;font-style:italic;'> {dto.SupplierName} </span>
                                    <span style='font-weight:bold;font-size:25px;font-style:italic;'> {dto.SupplierHouseNoAndStreetName},</span>
                                    <span style='font-weight:bold;font-size:25px;font-style:italic;'> {dto.SupplierTownAndCode}</span>
                                </div>

                            </div>

                        </div>

                        <table id='items' style='border-collapse:collapse;border-width:2px;border-color:black;width:100%;'>
                            <thead style='background-color:#ffe4e1;'>
                                <tr>
                                    <th style='{tdStyles}text-align:center;border-left-style:none;'>
                                        <span style='font-size:22px;font-weight:bold;'>Code</span>
                                    </th>
                                    <th style='{tdStyles}text-align:center;'>
                                        <span style='font-size:22px;font-weight:bold;'>Description</span>
                                    </th>
                                    <th style='{tdStyles}text-align:center;'>
                                        <span style='font-size:22px;font-weight:bold;'>Quantity</span>
                                    </th>
                                    <th style='{tdStyles}text-align:center;'>
                                        <span style='font-size:22px;font-weight:bold;'>Unit Price (R) <br />Inclusive</span>
                                    </th>
                                    <th style='{tdStyles}text-align:center;border-right-style:none;'>
                                        <span style='font-size:22px;font-weight:bold;'>Total Price (R) </span>
                                    </th>
                                </tr>
                            </thead>

                            <tbody>
                                {tableBuilder}
                            </tbody>

                        </table>

                        <div style='display:flex;flex-direction:row;flex-wrap:wrap;justify-content:space-evenly;padding:20px;border-top-style:none;
                            border-left-style:none;border-right-style:none;margin-top:15px;'>
                            <div>
                                <span style='font-size:22px;font-weight:bold;margin-right:6px;'>Ordered by:</span>
                                <span style='font-size:22px;font-style:italic;text-decoration:underline;'>{dto.OrderedBy}</span>
                            </div>
                            <div>
                                <span style='font-size:22px;font-weight:bold;margin-right:6px;'>Signature:</span>
                                <span style='font-size:22px;font-style:italic;text-decoration:underline;'>{dto.Signature}</span>
                            </div>
                        </div>

                    </div>
                ";

            string trHoverStyle = "#items tbody tr:hover{background-color:#faf0e6;}";

            string html =
                $@"
                    <!DOCTYPE html>
                    <html>
                        <head>
                            <title>Order</title>
                            <style type='text/css'>{trHoverStyle}</style>
                        </head>
                        <body>{body}</body>
                    </html>
                ";

            htmlBuilder.Append(html);
            htmlBuilder.Replace('$', ' ');

            // get the path where the file will be stored in the file system
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = Path.Combine(path, "Downloads", $"Order-{dto.Id}.html");

            // write the html to a file
            using StreamWriter writer = new(filePath, false, Encoding.UTF8);
            writer.Write(htmlBuilder);

            return dto;
        }

        return null;
    }

    /// <summary>
    /// Checks is a string is nor null or empty.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static bool IsStringValid(string? str) => !string.IsNullOrEmpty(str);
}