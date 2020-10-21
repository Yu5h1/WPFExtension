using System;
using System.IO;


public class PathInfo
{
    public string fullPath;
    public PathInfo(string FullPath) => fullPath = FullPath;
    public string Name {
        get => GetName(fullPath);
        set => fullPath = ChangeName(value);
    }
    public string ChangeName(string name) => Path.Combine(directory, name + Extension);
    public string FileName => GetFileName(fullPath);
    public string Extension => GetExtension(fullPath);
    public string directory => GetDirectory(fullPath);
    public string directoryName => GetFileName(directory);
    public void ChangeExtension(string extension) => fullPath = Path.ChangeExtension(fullPath, extension);
    public bool Exists => IsExists(fullPath);
    public string Root => GetRoot(fullPath);
    public bool IsDirectory {
        get {
            if (Exists) return Attributes.HasFlag(FileAttributes.Directory);
            return !Path.HasExtension(fullPath);
        }
    }
    public bool IsLocked => IsPathLocked(fullPath);
    public static bool IsPathLocked(string path) => GetPathLockedException(path) != null;
    public FileAttributes Attributes => File.GetAttributes(fullPath);

    public static implicit operator string(PathInfo pathInfo) => pathInfo.fullPath;


    public string CombineWith(params string[] paths)
    {
        string result = fullPath;
        foreach (var item in paths) Path.Combine(result, item);
        return result;
    }
    public string FindAny(params string[] paths)
    {
        foreach (var item in paths) { if (File.Exists(CombineWith(item))) return CombineWith(item); }
        return null;
    }

    public static string Combine(params string[] paths) => Path.Combine(paths);
    public static string GetName(string path) => Path.GetFileNameWithoutExtension(path);
    public static string GetFileName(string path) => Path.GetFileName(path);
    public static string GetExtension(string path) => Path.GetExtension(path);
    public static string GetDirectory(string path) => Path.GetDirectoryName(path);
    public static string GetRoot(string path) => Path.GetPathRoot(path);
    public static bool IsExists(string path) => File.Exists(path) || Directory.Exists(path);
    

    public static Exception GetPathLockedException(string path)
    {        
        if (!File.Exists(path)) return null;
        if (Directory.Exists(path))
        {
            foreach (var f in Directory.GetFiles(path))
            {
                var curResult = GetPathLockedException(f);
                if (curResult != null) return curResult;
            }
        }
        else if (File.Exists(path)) {
            try
            {
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                { stream.Close(); }
            }
            catch (Exception e) { return e; }
        }
        return null;
    }


    public override string ToString() => fullPath;
}

