using System;
using System.IO;
using log4net.Util;

namespace Utils.Logging
{
    public class EnvironmentFolderConverter : PatternConverter
    {
        protected override void Convert(TextWriter writer, object state)
        {
            Environment.SpecialFolder specialFolder;
            if (!Enum.TryParse(Option, true, out specialFolder))
                return;
            writer.Write(Environment.GetFolderPath(specialFolder));
        }
    }
}
