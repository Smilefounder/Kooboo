#! "net5.0"
#load "common.csx"

using static Common;

var publishDir = Path.Combine(BinDir, "Release", "net5.0-windows", "win-x64", "publish");

Log("start generate kooboo wpf zip");
ClearBefore();
CopyAdmin();
CopyPublish(publishDir);
Sign();
CopyLang();
Compress();
ClearAfter();
Log($"generate kooboo wpf zip finish {ZipPath}");

void Sign()
{
    var signToolPath = Path.Combine(SlnDir, "tools", "sign", "signtool.exe");
    var certPath = Path.Combine(SlnDir, "tools", "sign", "yardi.pfx");
    var password = "kooboo";
    var exePath = Path.Combine(KoobooDir, "Kooboo.App.exe");

    try
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = signToolPath,
            Arguments = $@"sign /f {certPath} /p ""{password}"" {exePath}"
        }).WaitForExit();

        Process.Start(new ProcessStartInfo
        {
            FileName = signToolPath,
            Arguments = $@"timestamp /t http://timestamp.digicert.com {exePath}"
        }).WaitForExit();

        Log($"sign {exePath} success");
    }
    catch (System.Exception e)
    {
        Log($"sign fail {e.ToString()}");
    }
}

