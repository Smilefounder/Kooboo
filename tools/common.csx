public static class Common
{
    public static void CopyDirectory(
      string source,
      string relativeTo,
      string target,
      string[] excludeDirs = null,
      string[] excludeExs = null,
      string[] includeExs = null)
    {
        var files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);

        foreach (var item in files)
        {
            var relativePath = Path.GetRelativePath(relativeTo, item);
            var relativeDir = Path.GetDirectoryName(relativePath);
            var targetPath = Path.Combine(target, relativePath);
            var extension = Path.GetExtension(targetPath)?.ToLower();
            if (excludeExs?.Any(a => a == extension) ?? false) continue;
            if (includeExs?.All(a => a != extension) ?? false) continue;
            if (excludeDirs?.Any(a => relativeDir.Contains(a)) ?? false) continue;
            var targetDir = Path.GetDirectoryName(targetPath);
            if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

            if (!File.Exists(targetPath))
            {
                Log($"copy '{item}' to '{targetPath}'");
                File.Copy(item, targetPath);
            }
        }
    }

    public static void Log(string log) => Console.WriteLine(log);
}