using InitializeRepos.Logic;

namespace InitializeRepos;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var standardsResponse = GitManager.PromptUser(
            "Do you want to pull down all Engineering Standards repos?");
        
        var engProjectsResponse = GitManager.PromptUser(
            "Do you want to pull down all Engineering Projects repos?");
        
        var teakProjectsResponse = GitManager.PromptUser(
            "Do you want to pull down all Teak Projects (Interdepartmental) repos?");
        
        var sikesPersonalProjectsResponse = GitManager.PromptUser(
            "Do you want to pull down DSikes Github Projects?");
     
        await GitHubDesktopManager.RemoveAllSettingsAndRepos();

        await FilesManager.ArchiveAllInSourceReposFolder();

        GitManager.PullDownAllRepos(standardsResponse, engProjectsResponse, teakProjectsResponse, sikesPersonalProjectsResponse);

        Console.WriteLine();
        Console.WriteLine("Finished!");
        Console.WriteLine("Press return to exit...");
        Console.ReadLine();
    }
}