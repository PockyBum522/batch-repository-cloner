using System.Text;
using InitializeRepos.Logic;
using LevelDB;

namespace InitializeRepos.Demo;

public class Program
{
    public static void Main(string[] args)
    {
        
    }

    private static async Task FilesManagerTestArchiveAll()
    {
        var sourceReposPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "source",
            "repos");

        // Make sure there's a few directories to archive

        Directory.CreateDirectory(
            Path.Join(sourceReposPath, "fh8347y8374fg3"));

        Directory.CreateDirectory(
            Path.Join(sourceReposPath, "83473284guh48yugh4g"));

        Directory.CreateDirectory(
            Path.Join(sourceReposPath, "245goiu9hf938fh348"));

        await FilesManager.ArchiveAllInSourceReposFolder();
    }
}