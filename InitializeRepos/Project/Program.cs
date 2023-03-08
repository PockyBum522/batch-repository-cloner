using InitializeRepos.Logic;
using InitializeRepos.Models;
using Newtonsoft.Json;

namespace InitializeRepos;

/// <summary>
/// Main entry point class
/// </summary>
public static class Program
{
    private static List<string> _jsonFilenamesToUse = new();
    
    /// <summary>
    /// Main entry point
    /// </summary>
    public static async Task Main(string[] args)
    {
        if (GitManager.PromptUser("Do you want to pull down DSikes Github Projects?"))
            _jsonFilenamesToUse.Add("PockyBum522 - GitHub.json");
     
        if (GitManager.PromptUser("Do you want to pull down all Orlando Science Center repos?"))
            _jsonFilenamesToUse.Add("Orlando Science Center - GitHub.json");
        
        // if (GitManager.PromptUser("Do you want to pull down all Engineering Standards repos?"))
        //     _jsonFilenamesToUse.Add("Teak - Engineering Standards.json");
        //
        // if (GitManager.PromptUser("Do you want to pull down all Engineering Projects repos?"))
        //     _jsonFilenamesToUse.Add("Teak - Engineering Projects.json");
        //     
        // if (GitManager.PromptUser("Do you want to pull down all Teak Projects (Interdepartmental) repos?"))
        //     _jsonFilenamesToUse.Add("Teak - Organizational Projects.json");
        
        await GitHubDesktopManager.RemoveAllSettingsAndReposInGitHubDesktop();

        await FilesManager.ArchiveAllInSourceReposFolder();

        GitManager.PullDownAllRepos(_jsonFilenamesToUse);

        Console.WriteLine();
        Console.WriteLine("Finished!");
        Console.WriteLine("Press return to exit...");
        Console.ReadLine();
    }
    
    /// <summary>
    /// Use this if you need an easy way to dump some example json
    /// </summary>
    /// <param name="reposInformation"></param>
    private static void SerializeToJson(OrganizationRepos reposInformation)
    {
        var serializer = new JsonSerializer
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        var pathToWriteTo = Path.Join( 
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "TestJson.json");
        
        using var jsonStateFileWriter = new StreamWriter(pathToWriteTo);
        
        using var jsonStateWriter = new JsonTextWriter(jsonStateFileWriter) { Formatting = Formatting.Indented };
        
        serializer.Serialize(jsonStateWriter, reposInformation);
    }
}