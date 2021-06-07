//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com
//All rights reserved.
using Kooboo.Lib;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Kooboo.App
{
    public class KoobooAutoStart
    {
        private readonly static string _taskName = "KoobooApp";
        private readonly static string _firstBoot = "FirstBoot";
        private readonly static string _path = System.Windows.Forms.Application.ExecutablePath;
        private readonly static string _dir = Path.GetDirectoryName(_path);

        public static bool IsFirstBoot()
        {
            try
            {

                return AppSettingsUtility.GetBool(_firstBoot, true)
                //compatible old code
                && !File.Exists(Path.Combine(_path, "_Admin", "View"));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsAutoStart()
        {
            try
            {
                string output = ExecuteSchtasksCmd($"/Query /TN {_taskName}", out bool success);
                return success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void AutoStart(bool auto)
        {
            try
            {
                if (auto)
                {
                    string taskConfig =
$@"<?xml version=""1.0"" encoding=""utf-16""?>
<Task version=""1.2"" xmlns=""http://schemas.microsoft.com/windows/2004/02/mit/task"">
  <Triggers>
    <LogonTrigger>
      <StartBoundary>2000-01-01T11:59:14</StartBoundary>
      <Enabled>true</Enabled>
    </LogonTrigger>
  </Triggers>
  <Principals>
    <Principal>
      <LogonType>InteractiveToken</LogonType>
      <RunLevel>HighestAvailable</RunLevel>
    </Principal>
  </Principals>
  <Settings>
    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>
    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>
    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>
    <ExecutionTimeLimit>PT0S</ExecutionTimeLimit>
    <Enabled>true</Enabled>
  </Settings>
  <Actions>
    <Exec>
      <Command>{_path}</Command>
      <WorkingDirectory>{_dir}</WorkingDirectory>
    </Exec>
  </Actions>
</Task>";
                    var dir = System.IO.Path.Combine(Kooboo.Data.AppSettings.RootPath, "AppData");
                    Kooboo.Lib.Helper.IOHelper.EnsureDirectoryExists(dir);

                    string path = Path.Combine(dir, "task.xml");
                    File.WriteAllText(path, taskConfig);

                    ExecuteSchtasksCmd($"/Create /TN {_taskName} /XML {path} /F", out bool success);
                }
                else
                {
                    ExecuteSchtasksCmd($"/Delete /TN {_taskName} /F", out bool success);
                }

                if (IsFirstBoot()) AppSettingsUtility.AddOrSave(_firstBoot, false.ToString());
            }
            catch (Exception)
            {
            }
        }

        private static string ExecuteSchtasksCmd(string arguments, out bool success)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "SCHTASKS";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                success = process.ExitCode == 0;
                return output;
            }
        }
    }
}