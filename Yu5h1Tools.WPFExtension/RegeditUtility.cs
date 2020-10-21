using System.Collections.Generic;
using Microsoft.Win32;

public static class RegeditUtility
{
    public static RegistryKey CreateSubKeyIfNull(RegistryKey key, string itemName) {
        RegistryKey subkey = key.OpenSubKey(itemName,true);
        if (subkey == null) subkey = key.CreateSubKey(itemName);
        return subkey;
    }
    public static RegistryKey AddSubMenu(RegistryKey key, string itemName)
    {
        RegistryKey subMenu = CreateSubKeyIfNull(key, itemName);
        subMenu.SetValue("SubCommands", ""); 
        subMenu.SetValue("MUIVerb", itemName);
        RegistryKey subMenuShell = CreateSubKeyIfNull(subMenu, "shell");
        return subMenuShell;
    }
    public static void AddMenuItem(RegistryKey shell, string itemPath, params object[] args)
    {
        itemPath = itemPath.Replace('/', '\\');
        string[] hierarchies = itemPath.Split('\\');
        string itemName = hierarchies[hierarchies.Length - 1];
        if (hierarchies.Length > 1)
        {
            for (int i = 0; i < hierarchies.Length - 1; i++)
            {
                shell = AddSubMenu(shell, hierarchies[i]);
            }                
        }
        RegistryKey Itemkey = CreateSubKeyIfNull(shell, itemName);
        Itemkey.SetValue("MUIVerb", itemName);
        RegistryKey commandKey = CreateSubKeyIfNull(Itemkey, "command");
        commandKey.SetValue("", "\"" + string.Join("\" \"", args) + "\"");
    }        
    public static void AddFolderMenuItem(string itemPath,string ApplicationPath, params object[] args)
    {
        List<object> FolderArgs = new List<object>(args);
        List<object> BackgroundArgs = new List<object>(args);
        FolderArgs.InsertRange(0,new object[] { ApplicationPath, "%1" });
        BackgroundArgs.InsertRange(0, new object[] { ApplicationPath, "%V" });
        AddMenuItem(Registry.ClassesRoot.OpenSubKey(@"Directory\shell", true), itemPath, FolderArgs.ToArray());
        AddMenuItem(Registry.ClassesRoot.OpenSubKey(@"Directory\Background\shell", true), itemPath, BackgroundArgs.ToArray());
    }
    public static bool DoesRegEditFolderMenuItemExist(string itemPath) {
        return Registry.ClassesRoot.OpenSubKey(@"Directory\shell\"+ itemPath) != null;
    }
         
    public static string GetShellPath(string itemPath) {
        itemPath = itemPath.Replace("/", @"\");
        var parts = itemPath.Split(new string[] { @"\" }, System.StringSplitOptions.None);
        string shellItemPath = parts[parts.Length - 1];
        if (parts.Length > 1)
        {
            for (int i = parts.Length - 2; i >= 0; i--)
            {
                shellItemPath = parts[i] + @"\shell\" + shellItemPath;
            }
        }
        return shellItemPath;
    }
    public static void DeleteFolderMenuTree(string itemPath)
    {
        itemPath = GetShellPath(itemPath);

        var shellDir = Registry.ClassesRoot.OpenSubKey(@"Directory\shell", true);
        var shellDirBackGround = Registry.ClassesRoot.OpenSubKey(@"Directory\Background\shell", true);

        if (shellDir.HasSubKey(itemPath))
            shellDir.DeleteSubKeyTree(itemPath);
        if (shellDirBackGround.HasSubKey(itemPath))
            shellDirBackGround.DeleteSubKeyTree(itemPath);
    }

    public static void AddFileTypeMenuItem(string type,string itemPath, string ApplicationPath, params object[] args)
    {
        type = type.Replace(".", "");
        string shellPath = @"SystemFileAssociations\." + type + @"\Shell";
        RegistryKey shell = Registry.ClassesRoot.OpenSubKey(shellPath, true);            
        if (shell == null) (type + " does not exists in regedit.").PromptWarnning();
        else {
            List<object> cmdArgs = new List<object>(args);
            cmdArgs.InsertRange(0, new object[] { ApplicationPath, "%1" });
            AddMenuItem(shell, itemPath, cmdArgs.ToArray());
        }
    }
    public static void DeleteFileTypeMenuItem(string type, string itemPath)
    {
        string shellPath = @"SystemFileAssociations\." + type + @"\Shell";
        RegistryKey shell = Registry.ClassesRoot.OpenSubKey(shellPath, true);
        if (shell == null) (type + " does not exists in regedit.").PromptWarnning();
        else
        {
            if (shell.OpenSubKey(itemPath) != null) shell.DeleteSubKeyTree(itemPath);            
        }
    }
    public static bool DoesFileTypeMenuItemExist(string type, string itemPath)
    {
        return Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations\." + type + @"\Shell\"+ itemPath) != null;
    }
    public static bool HasSubKey(this RegistryKey key,string subKeyPath)
    {
        subKeyPath = subKeyPath.Replace("/", @"\");
        var parts = subKeyPath.Split(new string[] { @"\" }, System.StringSplitOptions.None);
        var curKey = key;
        foreach (var item in parts)
        {
            var subkey = curKey.OpenSubKey(item);
            if (subkey != null) curKey = subkey;
            else return false;
        }
        return true;
    }
}
