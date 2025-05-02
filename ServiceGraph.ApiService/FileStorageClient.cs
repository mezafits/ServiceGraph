using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Newtonsoft.Json;
using System.Text;
 

public interface IJsonStorageClient
{
    Task StoreJsonAsync(string jsonContent, string relativePath);
}

public class FileStorageClientConfig
{
    public string BaseDirectory { get; set; } = "./"; // Default to current directory
}


public class FileStorageClient : IJsonStorageClient
{
    private readonly string _baseDirectory;

    public FileStorageClient(FileStorageClientConfig config)
    {
        _baseDirectory = config.BaseDirectory;
    }

    public FileStorageClient(IOptions<FileStorageClientConfig> config)
    {
        _baseDirectory = config.Value.BaseDirectory;
    }

    public async Task StoreJsonAsync(string jsonContent, string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentException("Path cannot be empty.", nameof(relativePath));

        string fullPath = Path.Combine(_baseDirectory, relativePath);

        // Ensure directory exists
        string? directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Write JSON content to file
        await File.WriteAllTextAsync(fullPath, jsonContent, Encoding.UTF8);
    }
}

//public class AzureDevOpsClientConfig
//{
//    public string uri { get; set; }
//    public string personalAccessToken { get; set; }
//    public string projectName { get; set; }
//    public string repositoryName { get; set; }
//}
//public class AzureDevOpsClient
//{
//    private readonly string _uri;
//    private readonly string _personalAccessToken;
//    private readonly string _projectName;
//    private readonly string _repositoryName;

//    public AzureDevOpsClient(AzureDevOpsClientConfig config)
//    {
//        s_uri = config.uri;
//        _personalAccessToken = config.personalAccessToken;
//        _projectName = config.projectName;
//        _repositoryName = config.repositoryName;
//    }
//    public AzureDevOpsClient(IOptions<AzureDevOpsClientConfig> config)
//    {
//        _uri = config.Value.uri;
//        _personalAccessToken = config.Value.personalAccessToken;
//        _projectName = config.Value.projectName;
//        _repositoryName = config.Value.repositoryName;
//    }

//    public async Task PushJsonToRepo(string jsonContent, string pathInRepo)
//    {
//        // Create the connection
//        var creds = new VssBasicCredential(string.Empty, _personalAccessToken);
//        var connection = new VssConnection(new Uri(_uri), creds);

//        // Get a GitHttpClient to talk to the Git endpoints
//        using var gitClient = await connection.GetClientAsync<GitHttpClient>();

//        // Get the default branch (you might want to specify the branch)
//        var repository = await gitClient.GetRepositoryAsync(_projectName, _repositoryName);
//        var refs = await gitClient.GetRefsAsync(repository.Id);
//        var mainRef = refs.Find(r => r.Name.EndsWith("main")); // or "master" or whatever your main branch is

//        // Create a push with a new commit containing the new file
//        var push = new GitPush
//        {
//            RefUpdates = new[] { new GitRefUpdate { Name = mainRef.Name, OldObjectId = mainRef.ObjectId } },
//            Commits = new[]
//            {
//                new GitCommitRef
//                {
//                    Comment = "Added DataCollection.json",
//                    Changes = new[]
//                    {
//                        new GitChange
//                        {
//                            ChangeType = VersionControlChangeType.Edit,
//                            Item = new GitItem { Path = $"/ServiceGraph/{pathInRepo}/DataCollection.json" },
//                            NewContent = new ItemContent
//                            {
//                                Content = jsonContent,
//                                ContentType = ItemContentType.RawText
//                            }
//                        }
//                    }
//                }
//            }
//        };

//        // Push the changes to the repository
//        await gitClient.CreatePushAsync(push, repository.Id);
//    }
//}
