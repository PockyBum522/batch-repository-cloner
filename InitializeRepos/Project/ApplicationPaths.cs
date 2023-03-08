namespace InitializeRepos;

public class ApplicationPaths
{
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
}