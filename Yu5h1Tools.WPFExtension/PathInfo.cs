using System;
using System.IO;


public class PathInfo
{
    public string fullPath;
    public PathInfo(string FullPath) => fullPath = FullPath;
    public string Name => Path.GetFileNameWithoutExtension(fullPath);
    public string FileName => Path.GetFileName(fullPath);
    public string Extension => Path.GetExtension(fullPath);
    public string directory => Path.GetDirectoryName(fullPath);
    public string directoryName => Path.GetFileName(directory);
    public void ChangeExtension(string extension) => fullPath = Path.ChangeExtension(fullPath, extension);
    public bool Exists => File.Exists(fullPath) || Directory.Exists(fullPath);
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

