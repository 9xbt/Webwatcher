using System.Windows.Forms;

namespace Webwatcher
{
    /// <summary>
    /// <see cref="Links"/> class
    /// </summary>
    internal static class Links
    {
        internal static readonly string AboutPage     = $@"file:///{Application.StartupPath.Replace("\\", "/")}/links/about.html";
        internal static readonly string SettingsPage  = $@"file:///{Application.StartupPath.Replace("\\", "/")}/links/settings.html";
        internal static readonly string ChangelogPage = $@"file:///{Application.StartupPath.Replace("\\", "/")}/links/changelog.html";
        internal static readonly string ErrorPage     = $@"file:///{Application.StartupPath.Replace("\\", "/")}/links/error.html";
    }
}
