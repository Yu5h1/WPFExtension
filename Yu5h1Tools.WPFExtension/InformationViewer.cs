#define WPF
using System;
using System.Diagnostics;
#if WPF
using System.Windows;
#else
using System.Windows.Forms;
#endif
using System.Collections.Generic;
using System.Linq;
using System.IO;

public static class InformationViewer
{
    public static void printbySeprateor(string separator, params object[] objs) {
        Console.WriteLine(string.Join(separator, objs));
    }
    static string CombineInfos(params object[] objs)
    {
        string infos = "";
        foreach (var obj in objs)
        {
            if (obj == null) infos += "Null";
            else infos += obj.ToString();
            infos += "\n";
        }
        return infos;
    }
    public static void print(params object[] objs)
    {
        Console.WriteLine(CombineInfos(objs));
    }
    public static void print(this object obj)
    {
        if (obj.GetType().IsArray)
            print((object[])obj);
        else { Console.WriteLine(obj.ToString()); }
    }
    public static string ToStringNullCheck(this object obj) => obj == null ? "Null" : obj.ToString();
    public static void PromptInfo(this object content)
    {
#if WPF
        MessageBox.Show(content.ToStringNullCheck(), "Informations", MessageBoxButton.OK, MessageBoxImage.Information);
#else
        MessageBox.Show(content.ToString(), "Informations", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif

    }
    public static void PromptInfos(string separator = "\n",params object[] obj)
    {
        PromptInfos(string.Join<object>("\n", obj));
    }
    public static bool PromptWarnning(this bool val,string content)
    {
        if (val) PromptWarnning(content);
        return val;
    }
    public static void PromptWarnning(this Exception e)
    {
        PromptWarnning(e.Message);
    }
    public static void PromptWarnning(object content)
    {
#if WPF
        MessageBox.Show(content.ToStringNullCheck(), "Warnning", MessageBoxButton.OK, MessageBoxImage.Warning);
#else
        MessageBox.Show(content.ToString(), "Warnning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
    }
    public static void PromptError(object content)
    {
#if WPF
        MessageBox.Show(content.ToStringNullCheck(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
#else
        MessageBox.Show(content.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif

    }
    public static bool IsFileLockedPrompt(string path)
    {
        try
        {
            using (var stream = File.Open(path,FileMode.Open, FileAccess.Read, FileShare.None))
            {
                stream.Close();
            }
        }
        catch (Exception e) {
            e.PromptWarnning();
            return true;
        }
        return false;
    }
}

