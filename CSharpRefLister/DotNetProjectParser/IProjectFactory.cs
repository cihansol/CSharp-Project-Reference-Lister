using System.IO;

namespace DotNetProjectParser
{
    /// <summary>
    /// Creates an instance of a project, regardless of target framework
    /// </summary>
    public interface IProjectFactory
    {
        /// <summary>
        /// Gets a new instance based on the provided file
        /// </summary>
        /// <param name="projectFile"></param>
        /// <returns></returns>
        Project GetProject(FileInfo projectFile);
    }
}