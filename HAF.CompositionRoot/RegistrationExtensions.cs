using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HAF.Domain;
using SimpleInjector;
using SimpleInjector.Advanced;

namespace HAF.CompositionRoot
{
    public static class RegistrationExtensions
    {
        /// <summary>Gets the files matching the specified pattern.</summary>
        /// <param name="pattern">
        ///     The pattern the files must match. The asterisk '*' and question mark '?' wild cards are
        ///     supported.
        /// </param>
        /// <returns>The full paths of the files matching the pattern.</returns>
        public static IEnumerable<string> GetFilesMatchingPattern(string pattern)
        {
            var path = NormalizePath(Path.GetDirectoryName(pattern));
            var fileName = Path.GetFileName(pattern);
            return Directory.GetFiles(path, fileName);
        }

        /// <summary>Gets the assemblies matching at least one of the specified patterns.</summary>
        /// <param name="patterns">The patterns. The asterisk '*' and question mark '?' wild cards are supported.</param>
        /// <returns>The assemblies matching any of the specified patterns.</returns>
        public static IEnumerable<Assembly> GetMatchingAssemblies(IEnumerable<string> patterns)
        {
            var files = patterns.SelectMany(GetFilesMatchingPattern);
            return files.Where(IsAssemblyFile).Select(TryLoadAssembly).Where(x => x != null);
        }

        /// <summary>Gets the assemblies matching the specified patterns.</summary>
        /// <param name="pattern">The pattern. The asterisk '*' and question mark '?' wild cards are supported.</param>
        /// <param name="additionalPatterns"></param>
        /// <returns>The assemblies matching any of the specified patterns.</returns>
        public static IEnumerable<Assembly> GetMatchingAssemblies(string pattern, params string[] additionalPatterns)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));
            if (additionalPatterns == null)
                throw new ArgumentNullException(nameof(additionalPatterns));
            var patterns = new List<string> { pattern };
            patterns.AddRange(additionalPatterns);
            return GetMatchingAssemblies(patterns);
        }

        /// <summary>Registers the types in the assemblies matching at least one of the specified patterns.</summary>
        /// <param name="container">The container.</param>
        /// <param name="patterns">
        ///     The patterns the assemblies must match at least one of. The asterisk '*' and question mark '?' wild cards are
        ///     supported.
        /// </param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static void RegisterCollectionWithoutComposite<T>(this Container container, params string[] patterns)
            where T : class
        {
            RegisterCollectionWithoutComposite<T>(container, GetMatchingAssemblies(patterns).ToArray());
        }

        public static void RegisterCollectionWithoutComposite<TService>(
            this Container container,
            params Assembly[] assemblies) where TService : class
        {
            var behavior = container.Options.ConstructorResolutionBehavior;

            var types = from assembly in assemblies
                        from type in assembly.GetTypes()
                        where typeof(TService).IsAssignableFrom(type)
                        where !type.IsAbstract
                        where !type.IsGenericTypeDefinition
                        where !IsCompositeOf<TService>(type, behavior)
                        select type;

            container.RegisterCollection<TService>(types);
        }

        /// <summary>Determines whether the specified <paramref name="extension" /> is an assembly extension.</summary>
        /// <param name="extension">The extension.</param>
        /// <returns>
        ///     <c>true</c> if the specified <paramref name="extension" /> is an assembly extension; otherwise,
        ///     <c>false</c>.
        /// </returns>
        private static bool IsAssemblyExtension(string extension) =>
            string.Equals(extension, ".dll", StringComparison.OrdinalIgnoreCase) || string.Equals(
                extension,
                ".exe",
                StringComparison.OrdinalIgnoreCase);

        /// <summary>Determines whether the specified file name represents a file that is a .NET assembly.</summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>
        ///     <c>true</c> if the specified file name represents a file that is a .NET assembly; otherwise,
        ///     <c>false</c>.
        /// </returns>
        private static bool IsAssemblyFile(string fileName) => IsAssemblyExtension(Path.GetExtension(fileName));

        private static bool IsCompositeOf<TService>(Type type, IConstructorResolutionBehavior behavior)
        {
            var ctor = behavior.GetConstructor(typeof(TService), type);

            return ctor.GetParameters().Any(p => p.ParameterType == typeof(IEnumerable<TService>));
        }

        /// <summary>Normalizes the path.</summary>
        /// <param name="path">The path.</param>
        /// <returns>An absolute path.</returns>
        private static string NormalizePath(string path)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Combine(PathHelpers.GetBaseDirectory(), path);
            return Path.GetFullPath(path);
        }

        private static Assembly TryLoadAssembly(string path)
        {
            try
            {
                return Assembly.LoadFrom(path);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}