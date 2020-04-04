using System.IO;

namespace Ultz.Muon
{
    internal class Program
    {
        // ReSharper disable once UnusedMember.Local
        private static void Main(FileInfo argument, FileInfo? output = null, Environment environment = Environment.Auto,  Verbosity verbosity = Verbosity.Normal, Configuration configuration = Configuration.Debug)
        {
            var config = new CompilerConfiguration(argument, output, verbosity, configuration, environment);
        }
    }
}
