namespace InitializeRepos.Models;

public class OrganizationRepos
{
    public string BaseOrganizationFolder { get; set; }
    
    public string OptionalCategorySubfolder { get; set; }
    
    public List<string> RepoUrlsToClone { get; set; }
}