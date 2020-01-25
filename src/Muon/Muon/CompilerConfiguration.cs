using System.IO;
using System.Runtime.InteropServices;
using Ultz.Muon.Utils;

namespace Ultz.Muon
{
    public class CompilerConfiguration
    {
        private const string OutputFolder = "/bin/";

        public CompilerConfiguration(FileInfo source, FileInfo? output, Verbosity verbosity, Configuration configuration, Environment environment)
        {
            if (output is null)
            {
                output = new FileInfo(OutputFolder + source.Name);
            }

            if (!source.Exists)
            {
                ThrowHelper.ThrowArgumentException(nameof(source), Resources.GetRes("Argument_FileDoesNotExist"));
            }

            ThrowHelper.ThrowIfNotDefined(environment, nameof(environment));
            ThrowHelper.ThrowIfNotDefined(verbosity, nameof(verbosity));
            ThrowHelper.ThrowIfNotDefined(configuration, nameof(configuration));

            Source = source;
            Output = output;
            Verbosity = verbosity;
            Configuration = configuration;
            Environment = environment == Environment.Auto ? GetCurrentEnvironment() : environment;
        }

        public FileInfo Source { get; }
        public FileInfo Output { get; }
        public Verbosity Verbosity { get; }
        public Configuration Configuration { get; }
        public Environment Environment { get; }

        private static Environment GetCurrentEnvironment()
        {
            if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return Environment.Win64;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return Environment.Linux64;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return Environment.Osx64;
                }
            }
            else if (RuntimeInformation.ProcessArchitecture == Architecture.X86)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return Environment.Win32;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return Environment.Linux32;
                }
            }

            ThrowHelper.ThrowNotSupportedException(null!);
            return default;
        }
    }
}