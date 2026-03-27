using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartInventory.Web.Models;

namespace SmartInventory.Web.Pages.Stock;

public class ViewStockTransactionsModel(HttpClient client) : PageModel
{
    private readonly HttpClient _client = client;

    [BindProperty]
    public string? IsFetched { get; set; } = "";

    /// <summary>
    /// Indicates where to start reading stock transactions.
    /// </summary>
    [BindProperty]
    public int Start { get; set; } = 0;

    /// <summary>
    /// Indicates where to start reading transaction grouped by reason.
    /// </summary>
    public int StartForReason { get; set; } = 0;

    /// <summary>
    /// Indicates how many stock transactions to read at a time.
    /// </summary>
    [BindProperty]
    public int Count { get; set; } = 15;

    /// <summary>
    /// Stores stock transactions to be displayed.
    /// </summary>
    [BindProperty]
    public List<StockTransactionDto>? StockTransactions { get; set; } = [];

    /// <summary>
    /// Used to fetch stock transactions grouped by the specified reason.
    /// </summary>
    /// <param name="reason"></param>
    /// <returns></returns>
    private async Task GetStockTransactions()
    {
        string? username = HttpContext.Session.GetString("Username");

        if (!string.IsNullOrEmpty(username))
        {
            if (HttpContext.Session.GetInt32("Start") != null && HttpContext.Session.GetString("Transactions") != null)
            {
                int? s = HttpContext.Session.GetInt32("Start");
                Start = s ?? 0;
                StockTransactions = JsonSerializer.Deserialize<List<StockTransactionDto>>(HttpContext.Session.GetString("Transactions")!);
            }

            HttpResponseMessage? resp = await _client.GetAsync($"http://192.168.43.172:5196/api/Stock/ViewStockTransactions/{username}/{Start}/{Count}");

            if (resp.IsSuccessStatusCode)
            {
                if (StockTransactions == null || StockTransactions.Count == 0)
                {
                    StockTransactions = JsonSerializer.Deserialize<List<StockTransactionDto>>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else if (StockTransactions != null && StockTransactions.Count > 0)
                {
                    List<StockTransactionDto>? temp =
                        JsonSerializer.Deserialize<List<StockTransactionDto>>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    if (temp != null && temp.Count > 0)
                        StockTransactions.AddRange(temp);
                }

                if (StockTransactions != null && StockTransactions.Count > 0)
                {
                    Start = StockTransactions.Count;
                    HttpContext.Session.SetInt32("Start", Start);
                    HttpContext.Session.SetString("Transactions", JsonSerializer.Serialize(StockTransactions));
                }

            }
            else
                IsFetched = "false";
        }
    }

    /// <summary>
    /// Fetches transactions with the given reason.
    /// </summary>
    /// <param name="reason"></param>
    /// <returns></returns>
    public async Task GetStockTransactionsByReason(string reason)
    {
        if (!string.IsNullOrEmpty(reason))
        {
            string? username = HttpContext.Session.GetString("Username");

            if (!string.IsNullOrEmpty(username))
            {
             
                /*if (HttpContext.Session.GetInt32("StartForReason") != null && HttpContext.Session.GetString("TransactionsByReason") != null)
                {
                    int? s = HttpContext.Session.GetInt32("StartForReason");
                    StartForReason = s ?? 0;
                    StockTransactions = JsonSerializer.Deserialize<List<StockTransactionDto>>(HttpContext.Session.GetString("TransactionsByReason")!);
                }*/

                HttpResponseMessage resp = await _client.GetAsync($"http://192.168.43.172:5196/api/Stock/ViewStockTransactionsByReason/{username}/{reason}");

                if (resp.IsSuccessStatusCode)
                {
                    /*if (StockTransactions == null || StockTransactions.Count == 0)
                    {*/
                        StockTransactions = JsonSerializer.Deserialize<List<StockTransactionDto>>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    /*}
                    else if (StockTransactions != null && StockTransactions.Count > 0)
                    {
                        if (IsGroupedByReason(reason))
                        {
                            var list = JsonSerializer.Deserialize<List<StockTransactionDto>>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                            if (list != null && list.Count > 0)
                                StockTransactions.AddRange(list!);
                        }
                        else
                        {
                            StockTransactions.Clear();
                            StartForReason = 0;

                            StockTransactions = JsonSerializer.Deserialize<List<StockTransactionDto>>(await resp.Content.ReadAsStringAsync(), new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                        }
                    }
                    
                    if (StockTransactions != null && StockTransactions.Count > 0)
                    {
                        StartForReason = StockTransactions.Count + 1;
                        HttpContext.Session.SetInt32("StartForReason", StartForReason);
                        HttpContext.Session.SetString("TransactionsByReason", JsonSerializer.Serialize(StockTransactions));
                    }*/
                }
            }
        }
    }
    
    /// <summary>
    /// Checks if the transactions list contains transactions with the same reason only.
    /// </summary>
    /// <param name="reason"></param>
    /// <returns></returns>
    private bool IsGroupedByReason(string reason)
    {
        foreach (StockTransactionDto transaction in StockTransactions!)
        {
            if (transaction.Reason != reason)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Used to fetch stock transactions grouped by the specified reason.
    /// </summary>
    /// <param name="reason"></param>
    /// <returns></returns>
    public async Task OnGet(string? reason)
    {
        if (!string.IsNullOrEmpty(reason))
            await GetStockTransactionsByReason(reason);
        else
            await GetStockTransactions();
    }
}