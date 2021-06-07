#! "net5.0"
#load "common.csx"

using static Common;

var publishDir = Path.Combine(BinDir, "Release", "net5.0", "publish");

Log("start generate kooboo zip");
ClearBefore();
CopyAdmin();
CopyPublish(publishDir);
CopyLang();
CopyRuntimes();
Compress();
ClearAfter();
Log($"generate kooboo zip finish {ZipPath}");

void CopyRuntimes()
{
    Log("start copy Runtimes");
    var runtimesDir = Path.Combine(publishDir, "runtimes");
    CopyDirectory(runtimesDir, publishDir, KoobooDir);
    Log("copy Runtimes finish");
}