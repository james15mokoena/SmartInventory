namespace SmartInventory.API.Domain.DTO;

public class RecordStockDto
{
    public string? SKU { get; set; } = "";

    public int Quantity { get; set; }

    public string? Username { get; set; } = "";

    public string? TransactionReason { get; set; } = "";
}