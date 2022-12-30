using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubdate.Models
{
    internal class InformationsModel
    {
        public Uri WebUrl { get; set; }
        public Version Version { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public Uri DownloadUrl { get; set; }
        public string Description { get; set; }
    }
}
