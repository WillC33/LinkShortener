using System.Security.Cryptography;
using System.Text;

namespace LinkShortener;

/// <summary>
/// Deals with shortening the links
/// </summary>
internal class ShortenerService
{
    /// <summary>
    /// Returns a hashed version of the link
    /// </summary>
    /// <param name="link">the link</param>
    /// <returns>the hash</returns>
    internal string ShortenLink(string link)
    {
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(link));

        //convert to hexadecimal string
        StringBuilder stringBuilder = new();
        foreach (byte b in hashBytes)
        {
            stringBuilder.Append(b.ToString("x2"));
        }

        //shortens to 8 chars, which should avoid collisions at this scale
        return stringBuilder.ToString()[..8];
    }
}