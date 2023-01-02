using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GitHubdate.Models;
using Newtonsoft.Json;

namespace GitHubdate.Services
{
    internal class GitHubService
    {
        private readonly HttpClient _client;
        private readonly string _gitHubUsername;
        private readonly string _project;
        internal GitHubService(string GitHubUsername, string Project)
        {
            _gitHubUsername = GitHubUsername;
            _project = Project;
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://api.github.com")
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.UserAgent.TryParseAdd("GitHubdate");
        }

        internal async Task<InformationsModel> FetchInfo()
        {
            var response = await _client.GetAsync($"repos/{_gitHubUsername}/{_project}/releases");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var info = JsonConvert.DeserializeObject<Release[]>(result);

                if (info == null)
                {
                    return null;
                }

                foreach (var release in info)
                {
                    if (release.Assets == null || release.Assets.Length == 0)
                    {
                        continue;
                    }

                    if (release.Prerelease == false && release.Draft == false)
                    {
                        if(release.TagName.StartsWith("v") || release.TagName.StartsWith("V"))
                        {
                            release.TagName = release.TagName.Substring(1);
                        }
                        try
                        {
                            return new InformationsModel()
                            {
                                Name = release.Name,
                                Description = release.Body,
                                DownloadUrl = release.Assets[0].BrowserDownloadUrl,
                                FileName = release.Assets[0].Name,
                                Version = new Version(release.TagName),
                                WebUrl = release.HtmlUrl
                            };
                        }
                        catch (FormatException)
                        {
                            continue;
                        }

                    }
                }
            }
            return null;
        }
    }
}
