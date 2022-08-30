using System.Diagnostics;
using Microsoft.VisualBasic;

namespace InitializeRepos.Logic;

public class GitManager
{
    private static string SourceReposPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "source", "repos");
    
    private const string EngineeringStandardsUrlsFileName = "EngineeringStandardsRepoUrls.csv";
    private const string EngineeringProjectsUrlsFileName = "EngineeringProjectsRepoUrls.csv";
    private const string TeakProjectsUrlsFileName = "TeakProjectsRepoUrls.csv";
    private const string SikesGithubProjectsUrlsFileName = "SikesPersonalGithubProjects.csv";
    
    private const string TeakTopLevelFolderName = "Teak Isle Repos";
    private const string SikesGithubTopLevelFolderName = "PockyBum522 Github";
    
    public static void PullDownAllRepos(
        bool standardsResponse, 
        bool engProjectsResponse, 
        bool teakProjectsResponse,
        bool sikesPersonalProjectsResponse)
    {
        Console.WriteLine();
        Console.WriteLine("You may need to sign into DevOps now.");
        Console.WriteLine("If so, a window will open in which to sign in.");
        Console.WriteLine("Remember this may not be your @teakisle.com email...");
        
        if (standardsResponse) PullDownEngineeringStandardsRepos();
        if (engProjectsResponse) PullDownEngineeringProjectsRepos();
        if (teakProjectsResponse) PullDownTeakProjectsRepos();
        if (sikesPersonalProjectsResponse) PullDownSikesProjectsRepos();
        
        // Open explorer window for each category to make batch adding to GitHub desktop easier
        if (standardsResponse) Process.Start("explorer", Path.Join(SourceReposPath, TeakTopLevelFolderName, "Engineering Standards"));
        if (engProjectsResponse) Process.Start("explorer", Path.Join(SourceReposPath, TeakTopLevelFolderName, "Engineering Projects"));
        if (teakProjectsResponse) Process.Start("explorer", Path.Join(SourceReposPath, TeakTopLevelFolderName, "Teak Projects"));
        if (sikesPersonalProjectsResponse) Process.Start("explorer", Path.Join(SourceReposPath, "PockyBum522 Github"));
        
        Console.WriteLine();
        Console.WriteLine("To add repos to GitHub Desktop, select all folders inside the folder(s) that just" +
                          "opened and drag them onto a GitHub Desktop window");
    }

    private static void PullDownSikesProjectsRepos()
    {
        var sikesGithubReposUrls = new List<string>();
        
        var fullPathToCsv = Path.Combine(
            Path.GetDirectoryName(Environment.ProcessPath) ?? "",
            SikesGithubProjectsUrlsFileName);

        foreach (var line in File.ReadAllLines(fullPathToCsv))
        {
            var trimmedLine = line.Replace(",", "");

            PullRepoTo(
                Path.Join(SourceReposPath, "PockyBum522 Github"),
                trimmedLine);
        }
    }

    private static void PullDownEngineeringStandardsRepos()
    {
        var engineeringStandardsReposUrls = new List<string>();
        
        var fullPathToCsv = Path.Combine(
            Path.GetDirectoryName(Environment.ProcessPath) ?? "",
            EngineeringStandardsUrlsFileName);

        foreach (var line in File.ReadAllLines(fullPathToCsv))
        {
            var trimmedLine = line.Replace(",", "");

            PullRepoTo(
                Path.Join(SourceReposPath, "Teak Isle Repos", "Engineering Standards"), 
                trimmedLine);
        }
    }

    private static void PullDownEngineeringProjectsRepos()
    {
        var engineeringStandardsReposUrls = new List<string>();

        var fullPathToCsv = Path.Combine(
            Path.GetDirectoryName(Environment.ProcessPath) ?? "",
            EngineeringProjectsUrlsFileName);
        
        foreach (var line in File.ReadAllLines(fullPathToCsv))
        {
            var trimmedLine = line.Replace(",", "");

            PullRepoTo(
                Path.Join(SourceReposPath, "Teak Isle Repos", "Engineering Projects"), 
                trimmedLine);
        }
    }
    
    private static void PullDownTeakProjectsRepos()
    {
        var engineeringStandardsReposUrls = new List<string>();

        var fullPathToCsv = Path.Combine(
            Path.GetDirectoryName(Environment.ProcessPath) ?? "",
            TeakProjectsUrlsFileName);
        
        foreach (var line in File.ReadAllLines(fullPathToCsv))
        {
            var trimmedLine = line.Replace(",", "");

            PullRepoTo(
                Path.Join(SourceReposPath, "Teak Isle Repos", "Teak Projects"), 
                trimmedLine);
        }
    }
    
    private static void PullRepoTo(string folderPath, string repoUrl)
    {
        if (string.IsNullOrEmpty(repoUrl)) return;
        
        var repoName = repoUrl.Split("/").Last(); 
        
        Console.WriteLine($"Getting repo at: {repoUrl}");

        var destinationPath = Path.Join(folderPath, repoName);
        
        var procInfo = new ProcessStartInfo();
        
        var argsString = $"clone {repoUrl} \"{destinationPath}\"";

        procInfo.Arguments = argsString;

        procInfo.FileName = "git";

        // Console.WriteLine($"FULL COMMAND: {procInfo.FileName} {procInfo.Arguments}");
        
        var proc = Process.Start(procInfo);

        proc?.WaitForExit();
    }

    public static bool PromptUser(string promptMessage)
    {
        Console.Write($"{promptMessage} Y/[N]?");
        
        var response = Console.ReadLine();

        Console.WriteLine();
        
        if (response?.ToLower().StartsWith("y") ?? false) return true;

        return false;
    }
}