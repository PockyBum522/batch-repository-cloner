using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using InitializeRepos.Logic;
using NUnit.Framework;

namespace InitializeRepos.IntegrationTests;

public class TeakPathsTests
{
    [SetUp]
    public void Setup()
    {
        // Make sure there's a few directories to archive
        var sourceReposPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "source",
            "repos");
        
        Directory.CreateDirectory(
            Path.Join(sourceReposPath, "fh8347y8374fg3"));        
        
        Directory.CreateDirectory(
            Path.Join(sourceReposPath, "83473284guh48yugh4g"));        
        
        Directory.CreateDirectory(
            Path.Join(sourceReposPath, "245goiu9hf938fh348"));
    }
    
    [Test]
    public async Task ArchiveAllInSourceReposFolder_WhenCalled_ShouldArchiveAll()
    {
       var sourceReposPath = Path.Combine(
           Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
           "source",
           "repos");

       await FilesManager.ArchiveAllInSourceReposFolder();

       Directory.GetDirectories(sourceReposPath).Length.Should().Be(1);
    }
}