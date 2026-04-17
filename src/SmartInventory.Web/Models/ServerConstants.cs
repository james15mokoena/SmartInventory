namespace SmartInventory.Web.Models;

public class ServerConstants
{
    /// <summary>
    /// The IP address of Itel04.
    /// </summary>
    private const string ItelIPAddress = "192.168.64.43";

    private const string SamsungGalaxyIPAddress = "192.168.43.172";

    /// <summary>
    /// The address of the server's API.
    /// </summary>
    public string ApiAddress { get; } = $"http://{SamsungGalaxyIPAddress}:5196/api";
}