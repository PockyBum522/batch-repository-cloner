namespace InitializeRepos.Logic;

public static class FilesManager
{
    public static async Task ArchiveAllInSourceReposFolder()
    {
        var foldersToMove = Directory.GetDirectories(ApplicationPaths.ReposBasePath);
        
        var formattedTime = DateTimeOffset.Now.ToString("yyyy-M-d_HH-m-s");
        
        var archiveFolderName = $"Archive_{formattedTime}";
        
        var archiveFolderPath = Path.Combine(
            ApplicationPaths.ReposBasePath,
            archiveFolderName);
        
        if (foldersToMove.Length > 0 || Directory.GetFiles(ApplicationPaths.ReposBasePath).Length > 0)
            Directory.CreateDirectory(archiveFolderPath);
        
        var timeoutCountdown = 20;

        while (timeoutCountdown-- > 0)
        {
            try
            {
                foreach (var folder in foldersToMove)
                {
                    var fullDestinationPath = Path.Join(archiveFolderPath, Path.GetFileName(folder));
                    
                    Console.WriteLine($"Moving: {folder} to {fullDestinationPath}");
                    
                    Directory.Move(folder, fullDestinationPath);
                }
                
                foreach (var file in Directory.GetFiles(ApplicationPaths.ReposBasePath))
                {
                    var fullDestinationPath = Path.Join(archiveFolderPath, Path.GetFileName(file));
                    
                    Console.WriteLine($"Moving: {file} to {fullDestinationPath}");
                    
                    File.Move(file, fullDestinationPath);
                }

                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not move folder to archive in source/repos, exception: {ex.Message}");

                await Task.Delay(1000);
            }    
        }
    }
}