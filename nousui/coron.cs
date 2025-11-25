public static string ConvertToQuotedString(string dbValue)
{
    if (string.IsNullOrWhiteSpace(dbValue)) return "";

    var parts = dbValue.Split(':');
    if (parts.Length < 2) return "";

    return string.Join(",", 
        parts[1]
        .Split(',')
        .Select(n => $"'{n}'"));
}