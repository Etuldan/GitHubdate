using System.Threading.Tasks;

internal class Program
{
    static async Task Main(string[] args)
    {
        var downloader = new GitHubdate.GitHubdate("Etuldan", "MidiControl", "/SILENT /NOCANCEL /NORESTART /FORCECLOSEAPPLICATIONS /SUPPRESSMSGBOXES", false);
        await downloader.Check();
    }
}
