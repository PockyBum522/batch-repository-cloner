using System.Diagnostics;
using Microsoft.VisualBasic;

namespace InitializeRepos.Logic;

public class GitManager
{
    private const string EngineeringStandardsUrlsFileName = "EngineeringStandardsRepoUrls.csv";
    private const string EngineeringProjectsUrlsFileName = "EngineeringProjectsRepoUrls.csv";
    private const string TeakProjectsUrlsFileName = "TeakProjectsRepoUrls.csv";
    
    private const string TeakTopLevelFolderName = "Teak Isle Repos";
    
    public static void PullDownAllRepos(bool standardsResponse, bool engProjectsResponse, bool teakProjectsResponse)
    {
        Console.WriteLine();
        Console.WriteLine("You may need to sign into DevOps now.");
        Console.WriteLine("If so, a window will open in which to sign in.");
        Console.WriteLine("Remember this may not be your @teakisle.com email...");
        
        if (standardsResponse) PullDownEngineeringStandardsRepos();
        if (engProjectsResponse) PullDownEngineeringProjectsRepos();
        if (teakProjectsResponse) PullDownTeakProjectsRepos();
        
        // Open folder in explorer
        var sourceReposPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "source",
            "repos",
            TeakTopLevelFolderName);

        // Open explorer window for each category to make batch adding to GitHub desktop easier
        if (standardsResponse) Process.Start("explorer", Path.Join(sourceReposPath, "Engineering Standards"));
        if (engProjectsResponse) Process.Start("explorer", Path.Join(sourceReposPath, "Engineering Projects"));
        if (teakProjectsResponse) Process.Start("explorer", Path.Join(sourceReposPath, "Teak Projects"));
        
        Console.WriteLine();
        Console.WriteLine("To add repos to GitHub Desktop, select all folders inside the folder(s) that just" +
                          "opened and drag them onto a GitHub Desktop window");
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

            PullDevOpsRepoTo(
                Path.Join(TeakTopLevelFolderName, "Engineering Standards"), 
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

            PullDevOpsRepoTo(
                Path.Join(TeakTopLevelFolderName, "Engineering Projects"), 
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

            PullDevOpsRepoTo(
                Path.Join(TeakTopLevelFolderName, "Teak Projects"), 
                trimmedLine);
        }
    }
    
    private static void PullDevOpsRepoTo(string folderName, string repoUrl)
    {
        if (string.IsNullOrEmpty(repoUrl)) return;

        var sourceReposPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "source",
            "repos");
        
        var repoName = repoUrl.Split("/").Last(); 
        
        Console.WriteLine($"Getting repo at: {repoUrl}");

        var destinationPath = Path.Join(sourceReposPath, folderName, repoName);
        
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