using System.Threading.Tasks;

internal class Program
{
    static async Task Main(string[] args)
    {
        var downloader = new GitHubdate.GitHubdate("Etuldan", "GitHubdate");
        var result = await downloader.Check();
        if (result == true)
        {
            await downloader.DownloadAndInstall();
        }
    }
}
