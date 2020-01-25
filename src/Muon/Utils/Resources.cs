using System.Resources;

namespace Ultz.Muon.Utils
{
    internal static class Resources
    {
        public static string GetRes(string name) => Strings.ResourceManager.GetString(name) ?? throw new MissingManifestResourceException();
    }
}