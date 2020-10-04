using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

public static class Win32Util
{
    private const int SW_SHOWNORMAL = 1;
    [DllImport("user32", CharSet = CharSet.Unicode)]
    static extern IntPtr FindWindow(string cls, string win);
    [DllImport("user32")]
    static extern IntPtr SetForegroundWindow(IntPtr hWnd);
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [DllImport("user32")]
    static extern bool IsIconic(IntPtr hWnd);
    [DllImport("user32")]
    static extern bool OpenIcon(IntPtr hWnd);

    public static void ForceSingleInstance(Application application)
    {
        var p = Process.GetCurrentProcess();
        var processes = Process.GetProcessesByName(p.ProcessName);
        if (processes.Length > 1) { 
            ShowExistingWindow();
            application.Shutdown();
        } 
    }
    static void ShowExistingWindow()
    {
        var currentProcess = Process.GetCurrentProcess();
        var processes = Process.GetProcessesByName(currentProcess.ProcessName);
        foreach (var process in processes)
        {
            // the single-instance already open should have a MainWindowHandle
            if (process.MainWindowHandle != IntPtr.Zero)
            {
                // restores the window in case it was minimized
                ShowWindow(process.MainWindowHandle, SW_SHOWNORMAL);

                // brings the window to the foreground
                SetForegroundWindow(process.MainWindowHandle);

                return;
            }
        }
    }
}
