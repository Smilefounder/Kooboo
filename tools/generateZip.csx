#! "net5.0"
#load "common.csx"

using static Common;

var publishDir = Path.Combine(BinDir, "Release", "net5.0", "publish");
Log("start generate kooboo zip");
Clear();
CopyAdmin();
CopyPublish(publishDir);
CopyLang();
CopyRuntimes();
Log("compress Kooboo.zip");
System.IO.Compression.ZipFile.CreateFromDirectory(KoobooDir, ZipPath);
Log($"delete {KoobooDir}");
Directory.Delete(KoobooDir, true);
Log($"delete {publishDir}");
Directory.Delete(publishDir, true);
Log($"generate kooboo zip finish {ZipPath}");

void CopyRuntimes()
{
    Log("start copy Runtimes");
    var runtimesDir = Path.Combine(publishDir, "runtimes");
    CopyDirectory(runtimesDir, publishDir, KoobooDir);
    Log("copy Runtimes finish");
}