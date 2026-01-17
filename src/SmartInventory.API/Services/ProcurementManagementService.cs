using System.Text;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Repositories;

namespace SmartInventory.API.Services;

/// <summary>
/// Defines the functionality for providing services performed by the purchase
/// department.
/// </summary>
public class ProcurementManagementService(ProcurementManagementRepository procRepo, PermissionManagementService premServ,
    UserManagementService userServ)
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
            string? vatRegNo = quote.VatRegNo;

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
}