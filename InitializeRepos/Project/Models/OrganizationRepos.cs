using JetBrains.Annotations;

namespace InitializeRepos.Models;

/// <summary>
/// Model for deserializing from the JSON files that hold the data about repo URLs and where to put them
/// </summary>
[PublicAPI]
public class OrganizationRepos
{
    /// <summary>
    /// Folder that gets put immediately into the repos base path. Usually the organization or person name
    /// </summary>
    public string BaseOrganizationFolder { get; set; } = "";
    
    /// <summary>
    /// If you want a subfolder in there, just make this not blank
    /// </summary>
    public string OptionalCategorySubfolder { get; set; } = "";

    /// <summary>
    /// List of URLs. Example formatting: https://github.com/OrlandoScienceCenter/exhibit-jumping-ring.git
    /// </summary>
    public List<string> RepoUrlsToClone { get; set; } = new();
}