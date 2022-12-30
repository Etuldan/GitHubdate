using GitHubdate.Models;
using GitHubdate.Services;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitHubdate
{
    public class GitHubdate
    {
        private readonly LocalService _serviceLocalInfo;
        private readonly GitHubService _serviceGitHubInfo;
        private InformationsModel _gitHubInformations;
        private readonly string _installArguments;
        private readonly bool _automaticUpdate;

        public GitHubdate(string Username, string Project, string InstallArguments = "", bool AutomaticUpdate = true)
        {
            _installArguments = InstallArguments;
            _automaticUpdate = AutomaticUpdate;
            _serviceLocalInfo = new LocalService();
            _serviceGitHubInfo = new GitHubService(Username, Project);
        }

        private async Task DownloadAndInstall()
        {
            var fullPath = Path.Combine(Path.GetTempPath(), _gitHubInformations.FileName);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_gitHubInformations.DownloadUrl);
                var streamToReadFrom = await response.Content.ReadAsStreamAsync();
                var fileStream = File.Create(fullPath);

                streamToReadFrom.CopyTo(fileStream);
                fileStream.Close();
                streamToReadFrom.Close();

                var startInfo = new ProcessStartInfo
                {
                    WorkingDirectory = Path.GetTempPath(),
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    FileName = fullPath,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = _installArguments
                };
                try
                {
                    var exeProcess = Process.Start(startInfo);
                    exeProcess?.WaitForExit();
                    File.Delete(fullPath);
                }
                catch
                {
                    // Log error.
                }
            }
        }

        public async Task Check()
        {
            _gitHubInformations = await _serviceGitHubInfo.FetchInfo();
            if( _gitHubInformations == null || _gitHubInformations.DownloadUrl == null || _gitHubInformations.FileName == null || _gitHubInformations.Version == null)
            {
                return;
            }
            if (_gitHubInformations.Version.CompareTo(_serviceLocalInfo.Version) > 0)
            {
                if (!_automaticUpdate)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    result = MessageBox.Show(
                        $"Current version: {_serviceLocalInfo.Version}\n" +
                        $"Available version: {_gitHubInformations.Version}\n\n" +
                        _gitHubInformations.Description, 
                        $"Update available { _gitHubInformations.Name}", 
                        buttons);
                    if (result == DialogResult.Yes)
                    {
                        await DownloadAndInstall();
                    }
                }
                else
                {
                    await DownloadAndInstall();
                }
            }
        }
    }
}
