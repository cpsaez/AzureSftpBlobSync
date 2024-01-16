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
            fullFileName=PutBeginSlash(fullFileName);

            var relative = Path.GetRelativePath(originSearchPath, fullFileName);
            relative = RemoveBeginSlash(relative);
            var result = Path.Combine(destinyPath, relative);
            return ReplaceSlashs(result);

        }

        public static string ReplaceSlashs(string path)
        {
            return path.Replace(@"\", @"/");
        }

        public static string RemoveBeginSlash(string path)
        {
            var result = ReplaceSlashs(path);
            if (result.StartsWith(@"/"))
            {
                result = result.Substring(1, result.Length - 1);
            }

            return result;
        }

        public static string PutBeginSlash(string path)
        {
            var result = ReplaceSlashs(path);
            if (!result.StartsWith(@"/"))
            {
                result = "/" + path;
            }

            return result;
        }

        public static string PutEndSlash(string path)
        {
            var result = ReplaceSlashs(path);
            if (!result.EndsWith("/"))
            {
                result = result + "/";
            }
            return result;
        }

        public static string PutBeginEndSlash(string path)
        {
            var result = PutBeginSlash(path);
            return PutEndSlash(result);
        }
    }
}
