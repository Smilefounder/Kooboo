#! "net5.0"
#load "common.csx"

using static Common;

var publishDir = Path.Combine(BinDir, "Release", "net5.0-windows", "win-x64", "publish");
var exePath = Path.Combine(publishDir, "Kooboo.App.exe");
var signToolPath = Path.Combine(SlnDir, "tools", "signtool.exe");
var certPath = Path.Combine(SlnDir, "tools", "codesign.p12");

Log("start generate kooboo wpf zip");
Sign();
// Clear();
// CopyAdmin();
// CopyPublish(publishDir);
// CopyLang();
// Log("compress Kooboo.zip");
// System.IO.Compression.ZipFile.CreateFromDirectory(KoobooDir, ZipPath);
// Log($"delete {KoobooDir}");
// Directory.Delete(KoobooDir, true);
// Log($"delete {publishDir}");
// Directory.Delete(publishDir, true);
// Log($"generate kooboo wpf zip finish {ZipPath}");


void Sign()
{
    Process.Start(new ProcessStartInfo
    {
        FileName = signToolPath,
        Arguments = $@"sign /f {certPath} /p ""kooboo"" {exePath}"
    }).WaitForExit();

    //  Process.Start(new ProcessStartInfo
    // {
    //     FileName = signToolPath,
    //     Arguments = $@"timestamp /t http://timestamp.digicert.com {exePath}"
    // }).WaitForExit();
}

