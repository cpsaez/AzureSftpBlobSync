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
            originSearchPath=PutBeginEndSlash(originSearchPath);
            destinyPath=PutBeginEndSlash(destinyPath);

            var relative = Path.GetRelativePath(originSearchPath, fullFileName);
            var result = Path.Combine(destinyPath, relative);
            return ReplaceSlashs(result);

        }

        private static string ReplaceSlashs(string path)
        {
            return path.Replace(@"\", @"/");
        }

        private static string PutBeginEndSlash(string path)
        {
            var result = ReplaceSlashs(path);
            if (!result.StartsWith(@"/")) {
                result = "/" + path;
            }
            if (!result.EndsWith("/"))
            {
                result = result + "/";
            }
            return result;
        }
    }
}
