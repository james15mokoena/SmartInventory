namespace SmartInventory.API.Services;

/// <summary>
/// Provides static methods for converting from base64 strings.
/// </summary>
public static class ConverterService
{
     /// <summary>
    /// Takes a base 64 string value and converts it into the original string value.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string FromBase64String(string str)
    {
        byte[] b = Convert.FromBase64String(str);
        char[] c = new char[b.Length];
        for (int i = 0; i < c.Length; ++i)
            c[i] = (char)b[i];

        return new string(c);
    }
}