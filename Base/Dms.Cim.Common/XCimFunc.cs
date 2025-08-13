using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Dms.Cim.Common
{
    public class XCimFunc
    {
        public static void IsDirectoryCheck(string Type, string UnitName)
        {
            DirectoryInfo dir = new DirectoryInfo(@"..\..\..\" + Type + @"\" + UnitName);

            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        public static void IsDirectoryCheck(string FilePath)
        {
            DirectoryInfo dir = new DirectoryInfo(FilePath);

            if (!dir.Exists)
            {
                dir.Create();
            }
        }


    }
}
