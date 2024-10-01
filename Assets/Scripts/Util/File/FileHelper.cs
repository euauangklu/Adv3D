using System.Linq;

namespace GDD.FileUtill
{
    public static class FileHelper
    {
        public static string GetFileNameFromPaths(string path)
        {
            var paths = path.Split("/").ToList();
            paths.Reverse();
            return paths[0];
        }
    }
}