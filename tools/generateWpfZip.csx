#! "net5.0"
#load "common.csx"

using static Common;

var publishDir = Path.Combine(BinDir, "Release", "net5.0-windows", "win-x64", "publish");
var exePath = Path.Combine(publishDir, "Kooboo.exe");
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
// Log($"generate kooboo wpf zip finish {ZipPath}");


void Sign()
{
     Process.Start(new ProcessStartInfo
    {
        FileName = signToolPath,
        Arguments = $@"sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /a {exePath}"
    }).WaitForExit();

    Log($"sign {exePath} success");
}

