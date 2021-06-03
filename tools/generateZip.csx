#! "net5.0"

Log("start generate kooboo linux zip");
var projectDir = Directory.GetCurrentDirectory();
Log($"project dir is {projectDir}");
var rootPath = new DirectoryInfo(projectDir).Parent.FullName;
var binDir = Path.Combine(projectDir, "bin");
var publishDir = Path.Combine(binDir, "Release", "net5.0", "publish");
var tempDir = Path.Combine(binDir, "Kooboo");
var zipPath = Path.Combine(binDir, "Kooboo.zip");

Clear();
CopyAdmin();
CopyPublish();
CopyLang();
CopyRuntimes();

void Log(string log) => Console.WriteLine(log);

void Clear()
{
    if (Directory.Exists(tempDir))
    {
        Directory.Delete(tempDir, true);
        Log($"the directory {tempDir} exists and has been deleted");
    }

    if (File.Exists(zipPath))
    {
        File.Delete(zipPath);
        Log($"the file {zipPath} exists and has been deleted");
    }
}

void CopyAdmin()
{
    Log("start copy _Admin");
    var excludes = new[] { "kbtest", ".vscode", "mobileEditor" };
    var webDir = Path.Combine(rootPath, "Kooboo.Web");
    var adminDir = Path.Combine(webDir, "_Admin");
    CopyDirectory(adminDir, webDir, tempDir, excludes);
    Log("copy _Admin finish");
}

void CopyPublish()
{
    Log("start copy Publish");
    var includes = new[] { ".dll", ".exe", ".config", ".json", ".zip" };
    CopyDirectory(publishDir, publishDir, tempDir, includeExs: includes);
    Log("copy Publish finish");
}

void CopyLang()
{
    Log("start copy Lang");
    var webDir = Path.Combine(rootPath, "Kooboo.Web");
    var langDir = Path.Combine(webDir, "Lang");
    CopyDirectory(langDir, webDir, tempDir);
    Log("copy Lang finish");
}

void CopyRuntimes()
{
    Log("start copy Runtimes");
    var runtimesDir = Path.Combine(publishDir, "runtimes");
    CopyDirectory(runtimesDir, publishDir, tempDir);
    Log("copy Runtimes finish");
}

Log("compress Kooboo.zip");
System.IO.Compression.ZipFile.CreateFromDirectory(tempDir, zipPath);
Log($"delete {tempDir}");
Directory.Delete(tempDir, true);
Log($"delete {publishDir}");
Directory.Delete(publishDir, true);


void CopyDirectory(
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
