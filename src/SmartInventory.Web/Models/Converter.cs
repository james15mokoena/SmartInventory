namespace SmartInventory.Web.Models;

/// <summary>
/// Provides static methods for converting to base64 string.
/// </summary>
public static class Converter
{
    /// <summary>
    /// Takes a string value and converts it into a base 64 string value.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToBase64String(string str)
    {
        byte[] b = new byte[str.Length];
        for (int i = 0; i < b.Length; ++i)
        {
            b[i] = (byte)str[i];
        }
        return Convert.ToBase64String(b);
    }
}