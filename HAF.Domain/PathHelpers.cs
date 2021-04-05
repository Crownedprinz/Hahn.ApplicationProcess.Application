using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace  HAF.Domain
{
    public static class PathHelpers
    {
        /// <summary>Gets the base directory for assemblies.</summary>
        /// <returns>The base directory from which to load the assemblies from.</returns>
        public static string GetBaseDirectory()
        {
            var baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (Directory.EnumerateFiles(baseDirectory).Any(x => Path.GetFileName(x) == "__AssemblyInfo__.ini"))
                baseDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            return baseDirectory;
        }
    }
}