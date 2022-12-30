using System;
using System.Diagnostics;
using System.IO;

namespace GitHubdate.Services
{
    internal class LocalService
    {
        internal Version Version { get; set; }
        internal LocalService()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 0)
            {
                try
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(args[0]);
                    Version = new Version(versionInfo.FileVersion);
                }
                catch (FileNotFoundException ex)
                { 
                }
                catch (Exception ex)    
                { 
                }
            }
        }
    }
}
