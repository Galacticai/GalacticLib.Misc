//  —————————————————————————————————————————————
//? 
//!? 📜 Paths.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: 
//      + (Galacticai) Platforms/Platform.cs
//? 
//  —————————————————————————————————————————————

using Commanders.Assets.Scripts.Lib.Platforms;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GalacticLib.Misc {
    /// <summary> Various tools for path (filesystem) manipulation </summary>
    public static class Paths {
        #region Extra
        /// <summary> Type of a path </summary>
        public enum PathType {
            None, File, Directory
        }
        /// <summary> Operating system of a path </summary>
        public enum PathOS {
            None, Windows, Unix
        }
        /// <summary> Regex matching a valid path string </summary>
        /// <value> Regex as <see cref="string" /> </value>
        public record PathRegex {
            /// <summary> Regex matching a path valid in Windows-based systems </summary>
            public const string WINDOWS
                = @"^(?<drive>[a-z]:)?(?<path>(?:[\\]?(?:[\w !#()-]+|[.]{1,2})+)*[\\])?(?<filename>(?:[.]?[\w !#()-]+)+)?[.]?$";
            /// <summary> Regex matching a path valid in Unix-based systems </summary>
            public const string UNIX
                = @"^(/[^/ ]*)+/?$";
        }

        #endregion


        #region Properties

        /// <returns> (ApplicationData) </returns>
        public static string ApplicationData
            => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        /// <returns><list type="bullet"> 
        /// <item> (ApplicationData)/(AppName) </item>
        /// <item> null (if the executing assembly name is not available) </item>
        /// </list></returns>
        #endregion


        #region Methods
        /// <summary> Create a directory for this application (called after the executing assembly name)
        /// <br/> Note: This cannot be done if the executing assembly name is not available </summary>

        /// <summary> Get a slash or a backslash depending on the currently running OS </summary>
        /// <returns> <list type="bullet">
        /// <item> Windows: '\' </item> <item> Linux: '/' </item>
        /// </list> </returns>
        public static char GetPathSlash()
        /// <summary> Get a slash or a backslash depending on <paramref name="pathOS"/> </summary>
        public static char GetPathSlash(PathOS pathOS)
            => pathOS == PathOS.Windows ? '\\' : '/';

        /// <summary> Determine the type of this <paramref name="path"/> matches </summary>
        public static PathType GetPathType(this string path) {
            if (File.Exists(path))
                return PathType.File;
            else if (Directory.Exists(path))
                return PathType.Directory;
            else return PathType.None;
        }
        /// <summary> Determine which OS path regex this <paramref name="path"/> matches </summary>
        public static PathOS GetPathOS(this string path) {
            if (Regex.IsMatch(path, PathRegex.WINDOWS))
                return PathOS.Windows;
            else if (Regex.IsMatch(path, PathRegex.UNIX))
                return PathOS.Unix;
            else return PathOS.None;
        }

        /// <summary> File or folder exists at the specified <paramref name="path"/> </summary>
        public static bool PathExists(this string path)
            => path.GetPathType() != PathType.None;
        /// <summary> The specified <paramref name="path"/> is a valid Windows or Unix path </summary>
        public static bool PathIsValid(this string path)
            => Regex.IsMatch(path, PathRegex.WINDOWS)
            || Regex.IsMatch(path, PathRegex.UNIX);
        /// <summary> The specified <paramref name="fileInfo"/> file contains a reparse point (is a symlink) </summary>
        public static bool PathIsSymbolic(this FileInfo fileInfo)
            => fileInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
        /// <summary> The specified <paramref name="path"/> contains a reparse point (is a symlink) </summary>
        public static string[] GetPathParts(this string path)
            => GetPathParts(path, path.GetPathOS());
        /// <summary> Split the <paramref name="path"/> to its parts
        /// <br/><br/> Example: 
        /// <br/> each/one/of/these/is/a/part/of/the/path </summary>
        /// <returns> ["each","one","of","these","is","a","part","of","the","path"] </returns> 
        public static string[] GetPathParts(this string path, PathOS pathOS)
            => pathOS switch {
                PathOS.Windows => path.Split('\\'),
                PathOS.Unix => path.Split('/'),
                _ => new[] { path }
            };

        /// <summary> Delete a path </summary>
        /// <param name="path"> Target path <see cref="string" /> </param>
        /// <returns> true if operation is complete </returns>
        public static bool DeletePath(string path) {
            if (!path.PathExists()) return true;
            //? invalid path: cannot delete
            if (!path.PathIsValid()) return false;

            PathType pathType = path.GetPathType();
            if (pathType == PathType.File)
                File.Delete(path);
            else if (pathType == PathType.Directory)
                Directory.Delete(path);
            return false;
        }

        /// <returns><list type="bullet">
        /// <item> "path/to/file (index).extension"</item>
        /// <item> "path/to/file (GUID).extension" if exceeding 20 tries </item>
        /// </list></returns>
        public static string GetUnusedPath(this string path)
            => GetUnusedPath(path, 20);
        /// <returns><list type="bullet">
        /// <item> "path/to/file (index).extension"</item>
        /// <item> "path/to/file (GUID).extension" if exceeding <paramref name="maxTries"/> </item>
        /// </list></returns>
        public static string GetUnusedPath(this string path, int maxTries) {
            if (!path.PathExists() || !path.PathIsValid())
                return path;
            string directory = Path.GetDirectoryName(path);
            string name = Path.GetFileNameWithoutExtension(path);
            string dot_extension = Path.GetExtension(path) ?? string.Empty;
            string newPath = string.Empty;
            for (int i = 1; i <= maxTries; ++i) {
                if (!newPath.PathExists()) return newPath;
                newPath = Path.Combine(directory, $"{name} ({i}){dot_extension}");
            }
            //? not returned in the loop = failed to get a unique name
            //?     > use GUID instead of numbers
            return Path.Combine(directory, $"{name} ({Guid.NewGuid()}){dot_extension}");
        }

        #endregion

    }
}
