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

    public static void ClearBefore()
    {
        if (Directory.Exists(KoobooDir))
        {
            Directory.Delete(KoobooDir, true);
            Log($"the directory {KoobooDir} exists and has been deleted");
        }

        if (File.Exists(ZipPath))
        {
            File.Delete(ZipPath);
            Log($"the file {ZipPath} exists and has been deleted");
        }
    }

    public static void ClearAfter()
    {
        if (Directory.Exists(KoobooDir))
        {
            Directory.Delete(KoobooDir, true);
            Log($"delete {KoobooDir}");
        }
    }

    public static void CopyAdmin()
    {
        Log("start copy _Admin");
        var excludes = new[] { "kbtest", ".vscode", "mobileEditor" };
        var webDir = Path.Combine(SlnDir, "Kooboo.Web");
        var adminDir = Path.Combine(webDir, "_Admin");
        CopyDirectory(adminDir, webDir, KoobooDir, excludes);
        Log("copy _Admin finish");
    }

    public static void CopyLang()
    {
        Log("start copy Lang");
        var webDir = Path.Combine(SlnDir, "Kooboo.Web");
        var langDir = Path.Combine(webDir, "Lang");
        CopyDirectory(langDir, webDir, KoobooDir);
        Log("copy Lang finish");
    }

    public static void CopyPublish(string publishDir)
    {
        Log("start copy Publish");
        var includes = new[] { ".dll", ".exe", ".config", ".json", ".zip" };
        CopyDirectory(publishDir, publishDir, KoobooDir, includeExs: includes);
        Log("copy Publish finish");
    }

    public static void Log(string log) => Console.WriteLine(log);

    public static void Compress()
    {
        Log($"compress {ZipPath}");
        System.IO.Compression.ZipFile.CreateFromDirectory(KoobooDir, ZipPath);
    }

    public static string ProjectDir => Directory.GetCurrentDirectory();

    public static string BinDir => Path.Combine(ProjectDir, "bin");

    public static string SlnDir => new DirectoryInfo(ProjectDir).Parent.FullName;

    public static string KoobooDir => Path.Combine(BinDir, "Kooboo");

    public static string ZipPath = Path.Combine(BinDir, "Kooboo.zip");
}