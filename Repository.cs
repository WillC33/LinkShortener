using Microsoft.Data.Sqlite;

namespace LinkShortener;
/// <summary>
/// Provides Db operations
/// </summary>
internal class Repository
{
    private const string ConnexionString = "Data Source=linkShortener.sqlite";

    /// <summary>
    /// Write a link to the Db
    /// </summary>
    /// <param name="link">the link model</param>
    internal void WriteLink(ShortenedLinkModel link)
    {
        try
        {
            using var connection = new SqliteConnection(ConnexionString);
            connection.Open();

            using var command = new SqliteCommand(
                "INSERT INTO ShortenedLinks (Hash, OriginalLink) VALUES (@Hash, @OriginalLink);", connection);
            
            command.Parameters.Add(new SqliteParameter("@Hash", SqliteType.Text) { Value = link.Hash });
            command.Parameters.Add(new SqliteParameter("@OriginalLink", SqliteType.Text) { Value = link.OriginalLink });
            
            command.ExecuteNonQuery();
            Console.WriteLine("Link inserted successfully.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inserting link: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Reads a link from the Db
    /// </summary>
    /// <param name="hash"></param>
    /// <returns></returns>
    internal ShortenedLinkModel ReadLink(string hash)
    {
        try
        {
            using var connection = new SqliteConnection(ConnexionString);
            connection.Open();

            using var command = new SqliteCommand(
                "SELECT * FROM ShortenedLinks WHERE Hash = @Hash;", connection);
            
            command.Parameters.AddWithValue("@Hash", hash);

            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null;
            
            return new ShortenedLinkModel
            { 
                Hash = reader["Hash"].ToString(), 
                OriginalLink = reader["OriginalLink"].ToString()
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error querying link: {ex.Message}");
        }   
    }
}
