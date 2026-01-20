using System.Text;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Repositories;

namespace SmartInventory.API.Services;

/// <summary>
/// Defines the functionality for providing services performed by the sales
/// department.
/// </summary>
public class SalesManagementService(SalesManagementRepository salesRepo,PermissionManagementService permServ, UserManagementService userServ)
{
    /// <summary>
    /// Used to interact with the database.
    /// </summary>
    private readonly SalesManagementRepository _salesRepo = salesRepo;

    /// <summary>
    /// Used to interact with the user management subsystem.
    /// </summary>
    private readonly UserManagementService _userService = userServ;

    /// <summary>
    /// Used to interact with the permission management subsystem.
    /// </summary>
    private readonly PermissionManagementService _permService = permServ;

    /// <summary>
    /// Used to generate the requisition form to be sent to the procurement department.
    /// </summary>
    /// <param name="formData"></param>
    /// <returns></returns>
    public RequisitionFormData? GenerateRequisitionForm(RequisitionFormData formData)
    {
        // some stock needs to be reordered.
        if (_permService.IsAuthorized(formData.Authorized!, "GenerateRequisitionForm") && IsRequisitionFormDataValid(formData) &&
            _salesRepo.AddRequisition(formData) is int id && id >= 0)
        {
            // set the document number to the one stored in the database.
            formData.DocNo = id;

            // the name of the person generating the stock report.
            string fullName = $"{_userService.GetStaffMember(formData.Authorized!)!.FirstName} {_userService.GetStaffMember(formData.Authorized!)!.LastName}";

            // set Authorized to the person's full name.
            formData.Authorized = fullName;

            // used to build the HTML document containing the Requisition form.
            StringBuilder htmlBuilder = new();

            // used to build a table of items to be reordered.
            StringBuilder tableBuilder = new();

            foreach (RequisitionDataItem item in formData.RequisitionDataItems)
            {
                string dataItem =
                    $@"
                        <tr>
                            <td style='border-style:solid;border-width:1px;border-color:black;border-left-style:none;'>
                                {item.Description}
                            </td>
                            <td style='border-style:solid;border-width:1px;border-color:black;'>
                                {item.Quantity}
                            </td>
                            <td style='border-style:solid;border-width:1px;border-color:black;'>
                                {item.Code}
                            </td>
                            <td style='border-style:solid;border-width:1px;border-color:black;'>
                                {item.SellingPrice}
                            </td>                            
                        </tr>
                    ";

                tableBuilder.Append(dataItem);
            }

            string body =
                $@"
                    <div class='main-container' style='border-style:solid;border-color:black;border-width:2px;
                        overflow-x:auto;overflow-y:auto;'>

                        <div class='top1-container' style='border-style:solid;border-top-style:none;border-left-style:none;
                            border-right-style:none;'>
                            <h1 style='text-align:center;'> REQUISITION </h1>                            
                            <h2 style='text-align:end;margin-right:15px;'>
                                <strong>No.: {formData.DocNo}</strong>
                            </h2>
                        </div>

                        <div class='top2-container' style='border-style:solid;border-left-style:none;border-right-style:none;border-top-style:none;'>
                            <h1 style='text-align:center;font-size:35px;'>
                                <strong>{formData.CompanyName}</strong>
                            </h1>

                            <h3 style='text-align:center;font-size:27px;'>
                                <strong>{formData.CompanyAddress}</strong>
                            </h3>
                        </div>

                        <div class='top3-container' style='display:flex;border-style:solid;border-left-style:none;border-right-style:none;border-top-style:none;
                            flex-direction:row;justify-content:space-evenly;height:80px;font-size:22px;'>
                            <span style='padding-right:7px;border-right-style:solid;border-left-style:none;border-top-style:none;border-bottom-style:none;'>
                                From Department:
                            </span>

                            <span style='padding-right:15px;text-align:start;padding-bottom:8px;border-right-style:solid;border-left-style:none;border-top-style:none;
                                border-bottom-style:none;flex-grow:0.3;'>
                                <strong><em>{formData.FromDepartment}</em></strong>
                            </span>
                            
                            <span style='padding-right:9px;text-align:start;padding-bottom:8px;border-right-style:solid;border-left-style:none;border-top-style:none;
                                border-bottom-style:none;flex-grow:0.05;'>
                                Date:
                            </span>

                            <span style='padding-right:5px;padding-bottom:8px;border-style:none;'>
                                <strong><em>{formData.DateGenerated}</em></strong>
                            </span>

                        </div>

                        <div class='requisition-table' style='margin-bottom:7px;margin-top:0;padding-top:0;border-top-style:none;font-size:22px;width:100%;overflow-x:auto;  
                            overflow-y:auto;'>

                            <table style='border-style:solid;border-width:1px;border-color:black;border-collapse:collapse;text-align:center;width:100%;
                                border-left-style:none;border-right-style:none;padding-top:0;margin-top:0;'>
                                <thead style='font-weight:bold;background-color:#deb887;'>
                                    <tr>
                                        <th style='border-style:solid;border-width:1px;border-color:black;border-left-style:none;'> Description of goods </th>
                                        <th style='border-style:solid;border-width:1px;border-color:black;'> Quantity </th>
                                        <th style='border-style:solid;border-width:1px;border-color:black;'> Code </th>
                                        <th style='border-style:solid;border-width:1px;border-color:black;'> Selling price (R)</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {tableBuilder}
                                </tbody>
                            </table>

                        </div>

                        <div class='authorised-container' style='margin-top:15px;margin-bottom:15px;margin-left:7px;font-size:22px;'>
                            <span style='margin-right:10px;'> Authorised:</span>
                            <span> <strong><em>{formData.Authorized}</em></strong> </span>
                        </div>

                    </div>                     
                ";

            string trHoverStyle = "tbody tr:hover{background-color:#87cefa;}";

            string html =
                $@"
                    <!DOCTYPE html>
                    <html>
                        <head>
                            <title> {formData.DocName} </title>
                            <style type='text/css'>
                                {trHoverStyle}
                            </style>
                        </head>

                        <body>
                            {body}
                        </body>

                    </html>
                ";

            htmlBuilder.Append(html);
            htmlBuilder.Replace('$', ' ');

            // generate an HTML file.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = Path.Combine(path, "Downloads", "Requisition.html");

            using StreamWriter htmlWriter = new(filePath, false, Encoding.UTF8);
            htmlWriter.Write(htmlBuilder);

            return formData;
        }

        return null;
    }

    /// <summary>
    /// Generates a tax invoice.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public TaxInvoiceDto? GenerateTaxInvoice(TaxInvoiceDto dto)
    {
        if (IsStringValid(dto.AccountNo) && IsStringValid(dto.CompanyBuldingNoAndStreetName) && IsStringValid(dto.CompanyPoBoxAddress) &&
            IsStringValid(dto.CompanyPoBoxTownAndCode) && IsStringValid(dto.CompanyTelephoneNo) && IsStringValid(dto.CompanyTownAndCode) &&
            IsStringValid(dto.CustomerHouseNoAndStreetName) && IsStringValid(dto.CustomerTownAndCode) && dto.CustomerVatRegNo != null &&
            IsStringValid(dto.MethodOfPayment) && dto.VatRegNo != null && dto.DateGenerated != default && dto.FaxNo != null &&
            dto.InvoiceItems.Count > 0 && _permService.IsAuthorized(dto.GeneratedBy,"GenerateTaxInvoice") && _salesRepo.AddTaxInvoice(dto) is
            int invoiceId && invoiceId >= 0)
        {

            // update the invoice id to the generated one
            dto.Id = invoiceId;

            StringBuilder htmlBuilder = new();

            StringBuilder tableBuilder = new();

            double totalPrice = 0;

            foreach (TaxInvoiceItemDto item in dto.InvoiceItems)
            {
                string tr =
                    $@"
                        <tr>
                            <td style='text-align:center;border-style:solid;border-width:2px;border-color:black;border-left-style:none;'>
                                <span style='text-align:center;font-size:22px;'>{item.Description}</span>
                            </td>
                            <td style='text-align:center;border-style:solid;border-width:2px;border-color:black;'>
                                <span style='text-align:center;font-size:22px;'>{item.Code}</span>
                            </td>
                            <td style='text-align:center;border-style:solid;border-width:2px;border-color:black;'>
                                <span style='text-align:center;font-size:22px;'>{item.Quantity}</span>
                            </td>
                            <td style='text-align:center;border-style:solid;border-width:2px;border-color:black;'>
                                <span style='text-align:center;font-size:22px;'>{item.UnitPrice}</span>
                            </td>
                            <td style='text-align:center;border-style:solid;border-width:2px;border-color:black;border-right-style:none;'>
                                <span style='text-align:center;font-size:22px;'>{item.TotalPrice}</span>
                            </td>
                        </tr>
                    ";

                totalPrice += item.TotalPrice;

                tableBuilder.Append(tr);
            }

            tableBuilder.Append(
                $@"
                    <tr>
                        <td colspan='4' style='text-align:end;padding-right:5px;border-style:solid;border-width:2px;border-color:black;
                            border-left-style:none;'>
                            <span style='font-size:25px;font-weight:bold;'>TOTAL INCLUSIVE 14% VAT </span>
                        </td>
                        <td colspan='1' style='text-align:center;border-style:solid;border-width:2px;border-color:black;border-right-style:none;'>
                            <span style='font-size:25px;font-weight:bold;'> {totalPrice} </span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan='4' style='text-align:start;padding-left:5px;border-style:solid;border-width:2px;border-color:black;
                            border-left-style:none;'>
                            <span style='font-size:25px;'>Amount tendered </span>
                        </td>
                        <td colspan='1' style='text-align:center;border-style:solid;border-width:2px;border-color:black;border-right-style:none;'>
                            <span> </span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan='4' style='text-align:start;padding-left:5px;border-style:solid;border-width:2px;border-color:black;
                            border-left-style:none;'>
                            <span style='font-size:25px;'>Change </span>
                        </td>
                        <td colspan='1' style='text-align:center;border-style:solid;border-width:2px;border-color:black;border-right-style:none;'>
                            <span> </span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan='5' style='text-align:end;padding-right:5px;border-style:solid;border-width:2px;border-color:black;
                            border-left-style:none;border-right-style:none;border-bottom-style:none;'>
                            <span style='font-size:25px;padding-right:5px;'>E. & O.E </span>
                        </td>                        
                    </tr>
                "
            );

            string body =
                $@"
                    <div style='overflow-x:auto;overflow-y:auto;border-style:solid;border-color:black;border-width:2px;border-bottom-style:none;'>

                        <div style='text-align:center;'>
                            <h1 style='text-align:center;font-size:50px;'>{dto.CompanyName.ToUpper()}</h1>
                            {(
                                !string.IsNullOrEmpty(dto.VatRegNo) ?
                                    "<p style='text-align:center;font-size:22px;'> Vat registration number: " + dto.VatRegNo + "</p>" :
                                    ""
                            )}
                        </div>

                        <div style='text-align:center;'>
                            <span style='font-size:22px;margin-bottom:5px;'>
                                {dto.CompanyBuldingNoAndStreetName}, {dto.CompanyPoBoxTownAndCode.Split(' ')[0]} - {dto.CompanyPoBoxAddress}, 
                                {dto.CompanyPoBoxTownAndCode}
                            </span> <br />
                            <span style='text-align:center;font-size:22px;margin-right:7px;'>
                                Tel: {dto.CompanyTelephoneNo}
                            </span>
                            {(
                                !string.IsNullOrEmpty(dto.FaxNo) ?
                                "<span style='text-align:center;font-size:22px;'> Fax: " + dto.FaxNo + "</span>" :
                                ""
                            )}
                        </div>

                        <div style='padding:8px;border-width:2px;border-style:solid;border-bottom-style:none;border-left-style:none;
                            border-right-style:none;margin-top:4px;'>
                            <h1 style='text-align:center;'> TAX INVOICE </h1>
                        </div>

                        <table style='border-collapse:collapse;border-width:2px;border-style:solid;border-color:black;width:100%;
                            border-left-style:none;border-right-style:none;'>
                            <tbody>
                                <tr>
                                    <td style='text-align:center;border-width:2px;border-style:solid;border-left-style:none;'>
                                        <span style='font-size:22px;font-weight:bold;'> Account No: {dto.AccountNo} </span>
                                    </td>
                                    <td style='text-align:center;border-width:2px;border-style:solid;'>
                                        <span style='font-size:22px;font-weight:bold;'>Invoice No: {dto.Id} </span>
                                    </td>
                                    <td style='text-align:center;border-width:2px;border-style:solid;'>
                                        <span style='font-size:22px;font-weight:bold;'>Date:</span>
                                    </td>
                                    <td style='text-align:center;border-width:2px;border-style:solid;border-right-style:none;'>
                                        <span style='font-size:22px;font-weight:bold;'> {dto.DateGenerated} </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan='2' rowspan='5' style='padding-left:7px;border-width:2px;border-style:solid;border-left-style:none;'>
                                        <span style='margin-top:20px;font-size:22px;font-weight:bold;'> Supplied to: </span>
                                        <div style='display:flex;flex-direction:column;flex-wrap:wrap;margin-top:13px;padding-left:20px;'>
                                            <span style='font-size:22px;font-weight:bold;'> {dto.CustomerName.ToUpper()}</span> <br />
                                            <span style='font-size:22px;font-weight:bold;'> {dto.CustomerHouseNoAndStreetName}</span> <br />
                                            <span style='font-size:22px;font-weight:bold;'> {dto.CustomerTownAndCode}</span> <br />
                                            {(
                                                !string.IsNullOrEmpty(dto.CustomerVatRegNo) ?
                                                "<span style='font-size:22px;font-weight:bold;'> VAT No: " + dto.CustomerVatRegNo + "</span> <br />" :
                                                ""
                                            )}
                                        </div>
                                    </td>
                                    <td colspan='2' rowspan='1' style='text-align:center;border-width:2px;border-style:solid;border-right-style:none;'>
                                        <span style='text-align:center;font-size:22px;font-weight:bold;'>Method of payment: </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='border-style:solid;border-color:black;border-width:2px;'>
                                        <span style='font-size:22px;font-weight:bold;padding:11px;'> Cheque </span>
                                    </td>
                                    <td style='border-style:solid;border-color:black;border-width:2px;border-right-style:none;'>
                                        <span style='font-size:22px;font-weight:bold;padding:11px;'> </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='border-style:solid;border-color:black;border-width:2px;'>
                                        <span style='font-size:22px;font-weight:bold;padding:11px;'> Cash </span>
                                    </td>
                                    <td style='border-style:solid;border-color:black;border-width:2px;border-right-style:none;'>
                                        <span style='font-size:22px;font-weight:bold;padding:11px;'> </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='border-style:solid;border-color:black;border-width:2px;'>
                                        <span style='font-size:22px;font-weight:bold;padding:11px;'> Credit Card </span>
                                    </td>
                                    <td style='border-style:solid;border-color:black;border-width:2px;border-right-style:none;'>
                                        <span style='font-size:22px;font-weight:bold;padding:11px;'> </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='border-style:solid;border-color:black;border-width:2px;'>
                                        <span style='font-size:22px;font-weight:bold;padding:11px;'> Account </span>
                                    </td>
                                    <td style='border-style:solid;border-color:black;border-width:2px;border-right-style:none;'>
                                        <span style='font-size:22px;font-weight:bold;padding:11px;'> </span>
                                    </td>
                                </tr>
                            </tbody>

                        </table>

                        <table style='border-collapse:collapse;border-width:2px;border-style:solid;border-color:black;width:100%;
                            border-left-style:none;border-right-style:none;border-top-style:none;'>

                            <thead>
                                <tr>
                                     <th style='text-align:center;border-style:solid;border-color:black;border-width:2px;border-top-style:none;
                                        border-left-style:none'>
                                        <span style='text-align:center;font-size:25px;font-weight:bold;'>Description</span>
                                    </th>
                                    <th style='text-align:center;border-style:solid;border-color:black;border-width:2px;border-top-style:none;'>
                                        <span style='text-align:center;font-size:25px;font-weight:bold;'>Code</span>
                                    </th>                                   
                                    <th style='text-align:center;border-style:solid;border-color:black;border-width:2px;border-top-style:none;'>
                                        <span style='text-align:center;font-size:25px;font-weight:bold;'>Quantity</span>
                                    </th>
                                    <th style='text-align:center;border-style:solid;border-color:black;border-width:2px;
                                        border-top-style:none;'>
                                        <span style='text-align:center;font-size:25px;font-weight:bold;'>Unit Price Incl (R)</span>
                                    </th>
                                    <th style='text-align:center;border-style:solid;border-color:black;border-width:2px;
                                        border-right-style:none;
                                        border-top-style:none;'>
                                        <span style='text-align:center;font-size:25px;font-weight:bold;'>Total Amount (R) </span>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>{tableBuilder}</tbody>

                        </table>

                    </div>
                ";

            string html =
                $@"
                    <!DOCTYPE html>
                    <html>
                        <head>
                            <title>Tax Invoice</title>
                            <style type='text/css'> </style>
                        </head>
                        <body>{body}</body>

                    </html>
                ";

            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = Path.Combine(path, "Downloads", $"TaxInvoice-{invoiceId}.html");

            using StreamWriter writer = new(filePath, false, Encoding.UTF8);
            writer.Write(html);

            return dto;
        }
        
        return null;
    }
    
    /// <summary>
    /// Checks if a string is not null or empty.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static bool IsStringValid(string str) => !string.IsNullOrEmpty(str);

    /// <summary>
    /// Verifies if the data in the requisition form satisfies the requirements.
    /// </summary>
    /// <param name="formData"></param>
    /// <returns></returns>
    private static bool IsRequisitionFormDataValid(RequisitionFormData formData) =>
        !string.IsNullOrEmpty(formData.Authorized) && !string.IsNullOrEmpty(formData.CompanyAddress) &&
        !string.IsNullOrEmpty(formData.CompanyName) && !string.IsNullOrEmpty(formData.DocName) &&
        !string.IsNullOrEmpty(formData.FromDepartment) && formData.DocNo >= 0 &&
        formData.DateGenerated != default && formData.RequisitionDataItems.Count > 0;
}