using System.Diagnostics;

namespace InitializeRepos.Logic;

/// <summary>
/// Methods for interacting with GitHub Desktop
/// </summary>
public static class GitHubDesktopManager
{
    /// <summary>
    /// Deletes all settings and repos currently stored in GitHub Desktop, note that this does
    /// not delete actual repos on the disk, just the information in GitHub Desktop
    /// </summary>
    internal static async Task RemoveAllSettingsAndReposInGitHubDesktop()
    {
        var githubDesktopLocalStoragePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "GitHub Desktop",
            "IndexedDB");

        var timeoutCountdown = 15;

        while (Directory.Exists(githubDesktopLocalStoragePath) &&
               timeoutCountdown-- > 0)
        {
            Console.WriteLine($"Deleting: {githubDesktopLocalStoragePath}");
            Console.WriteLine();

            try
            {
                Directory.Delete(githubDesktopLocalStoragePath, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"You likely have GitHub Desktop open, please close it now.");
                
                await Task.Delay(2000);    
            }
        }

        if (timeoutCountdown < 1)
        {
            Console.WriteLine("WARNING: Could not delete github desktop local storage.");
        }
    }
}