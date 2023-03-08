namespace InitializeRepos;

/// <summary>
/// Application paths that need to be hardcoded
/// </summary>
public class ApplicationPaths
{
    /// <summary>
    /// Repos base path. Checks if user is david, then checks if there's a D:\ drive
    /// Returns D:\source\repos if both are true, C:\source\repos otherwise
    /// </summary>
    public static string ReposBasePath
    {
        get
        {
            if (!Environment.UserDomainName.ToLower().Contains("david"))
            {
                return Path.Join(@"C:\source", "repos");
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement because this is easier to read
            if (Path.Exists(@"D:\Dropbox"))
            {
                return Path.Join(@"D:\source", "repos");
            }

            return Path.Join(@"C:\source", "repos");
        }
    }
    
    /// <summary>
    /// Per-user log folder path
    /// </summary>
    public static string LogAppBasePath =>
        Path.Combine(
            "C:",
            "Users",
            "Public",
            "Documents",
            "Logs",
            ApplicationData.ApplicationName);

    /// <summary>
    /// Actual log file path passed to the ILogger configuration
    /// </summary>
    public static string LogPath =>
        Path.Combine(
            LogAppBasePath,
            "Script.log");
    
    /// <summary>
    /// Assembly run from directory without the dll filename on the end
    /// </summary>
    public static string ThisApplicationRunFromDirectoryPath
        => Path.GetDirectoryName(Environment.ProcessPath) ?? "";
        
    /// <summary>
    /// Full process path to the dll that's running (This assembly)
    /// </summary>
    public static string ThisApplicationProcessPath 
        => Environment.ProcessPath ?? "";
}