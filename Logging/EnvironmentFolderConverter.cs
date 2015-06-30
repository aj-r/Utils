using System;
using System.IO;
using log4net.Util;

namespace SharpUtils.Logging
{
    /// <summary>
    /// A log4net PatternConverter that gets the path to a special folder, given the special folder name.
    /// </summary>
    public class EnvironmentFolderConverter : PatternConverter
    {
        /// <summary>
        /// Converts the special folder name to a path.
        /// </summary>
        /// <param name="writer">The writer to which the folder path will be written.</param>
        /// <param name="state">The current state.</param>
        protected override void Convert(TextWriter writer, object state)
        {
            Environment.SpecialFolder specialFolder;
            if (!Enum.TryParse(Option, true, out specialFolder))
                return;
            writer.Write(Environment.GetFolderPath(specialFolder));
        }
    }
}
