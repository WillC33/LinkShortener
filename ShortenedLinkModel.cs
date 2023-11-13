namespace LinkShortener;

/// <summary>
/// The link model for the Db
/// </summary>
internal record ShortenedLinkModel()
{
    internal string Hash { get; set; }
    internal string OriginalLink { get; set; }
}
