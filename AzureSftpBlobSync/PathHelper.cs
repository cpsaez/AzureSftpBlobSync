using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync
{
    public class PathHelper
    {
        public static string CalculateDestiny(string originSearchPath, string destinyPath, string fullFileName)
        {
            /*
             * Example:
             * originSearchPath = '/folder1/folder2/
             * destinyPath ='/incoming/toprocess
             * fullFileName = '/folder1/folder2/folder3/file.txt
             * result = '/incoming/toprocess/folder3/file.txt
             */

            var relative = Path.GetRelativePath(originSearchPath, fullFileName);
            var result=Path.Combine(destinyPath, relative);
            return result;

        }
    }
}
