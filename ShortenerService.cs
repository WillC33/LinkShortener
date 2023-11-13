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
        
        //should be sufficient to avoid collisions at this scale
        for (int i = 0; i < 8; i++)
        {
            stringBuilder.Append(hashBytes[i].ToString("x2"));
        }

        return stringBuilder.ToString();
    }
}