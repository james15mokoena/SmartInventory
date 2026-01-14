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
        if(!string.IsNullOrEmpty(quote.QuotedBy) && !string.IsNullOrEmpty(quote.SuppliedTo) && quote.DateGenerated != default &&
            quote.QuotationItems.Count > 0 && !string.IsNullOrEmpty(quote.Company) && !string.IsNullOrEmpty(quote.HouseNoAndStreetName) &&
            !string.IsNullOrEmpty(quote.TelephoneNo) && !string.IsNullOrEmpty(quote.TownOrSuburb) && !string.IsNullOrEmpty(quote.PoBoxAddress) &&
            !string.IsNullOrEmpty(quote.PostOfficeLocation) && _procRepo.AddQuotation(quote) is int quoteNo && quoteNo >= 0 &&
            _permServ.IsAuthorized(quote.QuotedBy,"GenerateQuotation"))
        {
            // set the name of the person for QuotedBy field
            quote.QuotedBy = $"{_userServ.GetStaffMember(quote.QuotedBy!)!.FirstName}  {_userServ.GetStaffMember(quote.QuotedBy!)!.LastName}";

            // used to build the HTML content
            StringBuilder htmlBuilder = new();

            // stores the vat registration number
            string? vatRegNo = quote.VatRegNo;

            string body =
                $@"
                    <div class='main-cont' style='border-style:solid;border-width:1px;border-color:black;overflow-x:auto;overflow-y:auto;'>

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

                        <div class='doc-name' style='border-style:solid;border-color:black;border-width:1px;border-left-style:none;
                            border-right-style:none;'>
                            <h2 style='text-align:center;padding:5px;height:60px;'> QUOTATION </h2>
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