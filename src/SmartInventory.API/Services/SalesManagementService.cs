using System.Text;
using SmartInventory.API.Domain.DTO;

namespace SmartInventory.API.Services;

/// <summary>
/// Defines the functionality for providing services performed by the sales
/// department.
/// </summary>
public class SalesManagementService(PermissionManagementService permServ, UserManagementService userServ)
{
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
        if (_permService.IsAuthorized(formData.Authorized!, "GenerateRequisitionForm") && IsRequisitionFormDataValid(formData))
        {

            // the name of the person generating the stock report.
            string fullName = $"{_userService.GetStaffMember(formData.Authorized!)!.FirstName} {_userService.GetStaffMember(formData.Authorized!)!.LastName}";

            // set Authorized to the person's full name.
            formData.Authorized = fullName;
            
            // used to build the HTML document containing the Requisition form.
            StringBuilder htmlBuilder = new();

            // used to build a table of items to be reordered.
            StringBuilder tableBuilder = new();

            foreach(RequisitionDataItem item in formData.RequisitionDataItems)
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