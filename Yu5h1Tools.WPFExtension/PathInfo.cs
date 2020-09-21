using System;
using System.IO;


public class PathInfo 
{
    public string fullPath;
    public PathInfo(string FullPath) => fullPath = FullPath;
    public string Name => Path.GetFileNameWithoutExtension(fullPath);
    public string FileName => Path.GetFileName(fullPath);
    public string Extension => Path.GetExtension(fullPath);
    public string Directory => Path.GetDirectoryName(fullPath);
    public string DirectoryName => Path.GetFileName(Directory);
    public void ChangeExtension(string extension) => fullPath = Path.ChangeExtension(fullPath,extension);
    public bool Exists => File.Exists(fullPath);
    public bool IsDirectory => File.GetAttributes(fullPath).HasFlag(FileAttributes.Directory);
    public bool IsLocked => IsFileLocked(fullPath);

    public static bool IsFileLocked(string path) => GetFileLockedException(path) != null;

    public static Exception GetFileLockedException(string path)
    {
        try
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                { stream.Close(); }
        }catch (Exception e) { return e; }
        return null;
    }
    public override string ToString() => fullPath;
}

