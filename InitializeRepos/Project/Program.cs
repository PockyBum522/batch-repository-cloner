using InitializeRepos.Logic;
using InitializeRepos.Models;
using Newtonsoft.Json;

namespace InitializeRepos;

public static class Program
{
    private static List<string> _jsonFilenamesToUse = new();
    
    public static async Task Main(string[] args)
    {
        var repoUrlsToClone = new List<string>()
        {
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/BatchTicketUtility",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/CutPacketMaker",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/EngineeringQueuers",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/FolderWatcherAndNotifier",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/InventorAddInManager",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/InventorKillAllInstances",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/InventorScreenshotTool",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/InventorWizard",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/NasCleaner",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/NotesPacketMaker",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/PacketAutoPrinter",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/ProductionMicaDatabaseClient",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/QuickUtilities",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/RouterCimMaterialsClient",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/RouterCimMaterialScrapManager",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/RouterCimMaterialsDatabasesSynchronizer",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/RouterCimRunScrapReportMaker",
            @"https://teakisle@dev.azure.com/teakisle/EngineeringProjects/_git/TeakTools"
        };

        var repoExample = new OrganizationRepos()
        {
            BaseOrganizationFolder = "Teak Isle",
            OptionalCategorySubfolder = "Engineering Projects",
            RepoUrlsToClone = repoUrlsToClone
        };

        SerializeToJson(repoExample);
        
        return;
        
        if (GitManager.PromptUser("Do you want to pull down DSikes Github Projects?"))
            _jsonFilenamesToUse.Add("SikesPersonalGithubProjects.json");
     
        if (GitManager.PromptUser("Do you want to pull down all Orlando Science Center repos?"))
            _jsonFilenamesToUse.Add("OrlandoScienceCenterRepoUrls.json");
        
        // if (GitManager.PromptUser("Do you want to pull down all Engineering Standards repos?"))
        //     _jsonFilenamesToUse.Add(".json");
        //
        // if (GitManager.PromptUser("Do you want to pull down all Engineering Projects repos?"))
        //     _jsonFilenamesToUse.Add(".json");
        //     
        // if (GitManager.PromptUser("Do you want to pull down all Teak Projects (Interdepartmental) repos?"))
        //     _jsonFilenamesToUse.Add(".json");
        
        await GitHubDesktopManager.RemoveAllSettingsAndReposInGitHubDesktop();

        await FilesManager.ArchiveAllInSourceReposFolder();

        GitManager.PullDownAllRepos(_jsonFilenamesToUse);

        Console.WriteLine();
        Console.WriteLine("Finished!");
        Console.WriteLine("Press return to exit...");
        Console.ReadLine();
    }
    
    public static void SerializeToJson(OrganizationRepos reposInformation)
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