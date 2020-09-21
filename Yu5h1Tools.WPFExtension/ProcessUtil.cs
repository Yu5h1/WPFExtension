using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using static InformationViewer;

public static class ProcessUtil
{
    public static string argsToLine(object[] args)
    {
        string argsLine = " ";
        foreach (var arg in args) argsLine += "\"" + arg.ToString() + "\" ";
        return argsLine;
    }

    public static bool Launch(string exe, Action<string> exited, params object[] args)
    {
        if (!File.Exists(exe))
        {
            (exe + " does not exists.").PromptWarnning();
            return false;
        }
        string argsLine = argsToLine(args);
        Process p = new Process();
        if (exited != null)
        {
            p.EnableRaisingEvents = true;
            p.Exited += (s, e) => exited(p.StandardOutput.ReadToEnd());
        }
        p.StartInfo = new ProcessStartInfo(exe, argsLine)
        {
            UseShellExecute = false,
            RedirectStandardOutput = true
        };
        return p.Start();
    }
    public static bool Launch(string exe, params object[] args)
    { return Launch(exe, null, args); }
    public static bool Launch(string exe, List<string> args)
    { return Launch(exe, null, args.ToArray()); }
    public static int LaunchWithExitCode(string exe, params object[] args)
    {
        var proc = Process.Start(exe, argsToLine(args));
        proc.WaitForExit();
        return proc.ExitCode;
    }
    public static string LaunchAndWaitOutPut(string exe, params object[] args)
    {
        var proc = Process.Start(exe, argsToLine(args));
        proc.WaitForExit();
        return proc.StandardOutput.ReadToEnd();
    }
    public static bool ShowInExplorer(this string path)
    {
        if (path != string.Empty)
        {
            if (Directory.Exists(path) || File.Exists(path))
            {
                string args = "\"" + path + "\"";
                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory)) Process.Start(args);
                else Process.Start("explorer.exe", "/select, " + args);

                return true;
            }
            else (path + "\ndoes not exists !").PromptWarnning();

        }
        else "empty path".PromptWarnning();
        return false;
    }
}
