using System.Diagnostics;
using System.Text.Json;
using InitializeRepos.Models;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using JsonException = Newtonsoft.Json.JsonException;

namespace InitializeRepos.Logic;

/// <summary>
/// Helper methods for working with git
/// </summary>
public class GitManager
{
    /// <summary>
    /// Pulls down all repos from a list of JSON files
    /// </summary>
    /// <param name="jsonFilesToUse">List of JSON filenames to pull repos from, serialized from OrganizationRepos</param>
    public static void PullDownAllRepos(List<string> jsonFilesToUse)
    {
        Console.WriteLine();
        Console.WriteLine("You may need to sign into DevOps now.");
        Console.WriteLine("If so, a window will open in which to sign in.");
        Console.WriteLine("Remember this may not be your @teakisle.com email...");

        foreach (var jsonFileName in jsonFilesToUse)
        {
            var fullJsonPath = Path.Join(
                ApplicationPaths.ThisApplicationRunFromDirectoryPath,
                "Repo Configurations");

            var reposInfo = LoadJsonOrganizationalInformationFromDisk(
                Path.Join(fullJsonPath, jsonFileName));

            PullDownAllRepos(reposInfo);

            string reposInfoFinalPath;

            if (string.IsNullOrWhiteSpace(reposInfo.OptionalCategorySubfolder))
                reposInfoFinalPath = Path.Join(ApplicationPaths.ReposBasePath, reposInfo.BaseOrganizationFolder);
            else
                reposInfoFinalPath = Path.Join(ApplicationPaths.ReposBasePath, reposInfo.BaseOrganizationFolder, reposInfo.OptionalCategorySubfolder);
            
            // Open explorer window for each category to make batch adding to GitHub desktop easier
            Process.Start("explorer", reposInfoFinalPath);
        }
        
        Directory.CreateDirectory(
            Path.Join(
                ApplicationPaths.ReposBasePath,
                "Non-GitHub Projects"));
        
        Console.WriteLine();
        Console.WriteLine("To add repos to GitHub Desktop, select all folders inside the folder(s) that just" +
                          "opened and drag them onto a GitHub Desktop window");
    }

    private static void PullDownAllRepos(OrganizationRepos repoInformationToClone)
    {
        string reposInfoFinalPath;
        
        if (string.IsNullOrWhiteSpace(repoInformationToClone.OptionalCategorySubfolder))
            reposInfoFinalPath = Path.Join(ApplicationPaths.ReposBasePath, repoInformationToClone.BaseOrganizationFolder);
        else
            reposInfoFinalPath = Path.Join(ApplicationPaths.ReposBasePath, repoInformationToClone.BaseOrganizationFolder, repoInformationToClone.OptionalCategorySubfolder);
        
        foreach (var repoUrl in repoInformationToClone.RepoUrlsToClone)
        {
            PullRepoTo(reposInfoFinalPath,repoUrl);
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

    /// <summary>
    /// Prompts the user, asking if they want to pull down a certain organization or category's repos
    /// </summary>
    /// <param name="promptMessage"></param>
    /// <returns></returns>
    public static bool PromptUser(string promptMessage)
    {
        Console.Write($"{promptMessage} Y/[N]?");
        
        var response = Console.ReadLine();

        Console.WriteLine();
        
        if (response?.ToLower().StartsWith("y") ?? false) return true;

        return false;
    }

    private static OrganizationRepos LoadJsonOrganizationalInformationFromDisk(string jsonFilePath)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        
        if (File.Exists(jsonFilePath))
        {
            var jsonStateRaw = File.ReadAllText(jsonFilePath);
        
            var jsonLoaded =
                JsonConvert.DeserializeObject<OrganizationRepos>(jsonStateRaw, settings) ??
                new OrganizationRepos();

            return jsonLoaded;
        }

        throw new JsonException();
    }
}