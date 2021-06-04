#! "net5.0"
#load "common.csx"

using static Common;

Log("start generate kooboo wpf zip");
var projectDir = Directory.GetCurrentDirectory();
Log($"project dir is {projectDir}");
var rootPath = new DirectoryInfo(projectDir).Parent.FullName;
var binDir = Path.Combine(projectDir, "bin");
var publishDir = Path.Combine(binDir, "Release", "net5.0", "publish");
var tempDir = Path.Combine(binDir, "Kooboo");
var zipPath = Path.Combine(binDir, "KoobooWpf.zip");

Clear();
CopyAdmin();
CopyPublish();
CopyLang();
Log("compress KoobooWpf.zip");
System.IO.Compression.ZipFile.CreateFromDirectory(tempDir, zipPath);
Log($"delete {tempDir}");
Directory.Delete(tempDir, true);
Log($"delete {publishDir}");
Directory.Delete(publishDir, true);
Log("generate kooboo wpf zip finish");


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