using Uri = System.Uri;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Debug = System.Diagnostics.Debug;
using System.Runtime.InteropServices;
using System;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;
using System.Windows.Interop;
using System.Threading.Tasks;
using System.Net;

namespace Yu5h1Tools.WPFExtension
{
    public static class ImageEx
    {
        //public static async void LoadImage(this Image image, string path) {
        //    var httpClient = new HttpClient();
        //    var a = await httpClient.GetByteArrayAsync(path);            
        //} 

        public static void LoadImage(this Image img, string path,
            EventHandler<ExceptionEventArgs> Failed = null,
            EventHandler<DownloadProgressEventArgs> Loading = null,
            EventHandler Completed = null,
            BitmapCreateOptions CreateOptions = BitmapCreateOptions.PreservePixelFormat,
            BitmapCacheOption CacheOption = BitmapCacheOption.OnLoad) {

            Uri targetURI = new Uri(path);
            img.Tag = path;

            bool IsLocalFile = Path.GetPathRoot(path) != "";

            if (IsLocalFile)
            {
                var bs = new BitmapImage(targetURI);
                img.Source = bs;
                Completed(bs,null);
            }
            else {
                var bitmapSource = new BitmapImage(targetURI);
                bitmapSource.DownloadFailed += Failed;
                bitmapSource.DownloadProgress += Loading;
                bitmapSource.DownloadCompleted += Completed;
            }
        }
        public static BitmapImage Create(Stream stream,
                    BitmapCreateOptions CreateOptions = BitmapCreateOptions.PreservePixelFormat,
                    BitmapCacheOption CacheOption = BitmapCacheOption.OnLoad) {
            var sourceImg = new BitmapImage();
            sourceImg.BeginInit();
            sourceImg.StreamSource = stream;
            sourceImg.CacheOption = CacheOption;
            sourceImg.CreateOptions = CreateOptions;
            sourceImg.EndInit();
            return sourceImg;
        }
        public static void LoadImage(this Image img, Stream stream,
        BitmapCreateOptions CreateOptions = BitmapCreateOptions.PreservePixelFormat,
        BitmapCacheOption CacheOption = BitmapCacheOption.OnLoad)
        {
            img.Source = Create(stream, CreateOptions, CacheOption);
        }
        public static void LoadImage(this Image image, byte[] bytes)
        {
            if (bytes != null && bytes.Length > 0) {
                var bitImg = new BitmapImage();
                bitImg.BeginInit();
                bitImg.CacheOption = BitmapCacheOption.OnLoad;
                bitImg.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitImg.StreamSource = new MemoryStream(bytes);
                bitImg.UriSource = null;
                bitImg.EndInit();
                image.Source = bitImg;
            }
        }
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        public static BitmapImage GetBitmapSource(this Image img)
        {
            if (img.Source.GetType() == typeof(BitmapFrame) || img.Source.GetType().BaseType == typeof(BitmapFrame))
            { return (BitmapImage)((ImageSource)img.Source); }
            return (BitmapImage)img.Source;
        }

        public static MemoryStream GetStream(this BitmapFrame from)
        {
            MemoryStream result = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(from);
            encoder.Save(result);
            return result;
        }
        public static MemoryStream GetStream(this BitmapSource from)
        {
            return BitmapFrame.Create(from).GetStream();
        }

        public static Stream GetSourceStream(this Image img)
        {
            
            if (img.Source.GetType() == typeof(BitmapFrame) || img.Source.GetType().BaseType == typeof(BitmapFrame))
            { return ((BitmapFrame)img.Source).GetStream(); }
            else return ((BitmapImage)img.Source).StreamSource;
        }

        //public static string GetImageFileExtension(this ImageFormat format) {
        //    if (format.Equals(ImageFormat.Jpeg)) return ".jpg";
        //    if (format.Equals(ImageFormat.Icon)) return ".ico";
        //    if (format.Equals(ImageFormat.Gif)) return ".gif";
        //    if (format.Equals(ImageFormat.Png)) return ".png";
        //    if (format.Equals(ImageFormat.Bmp)) return ".Bmp";
        //    return "";
        //}
        //public static string GetImageFileExtension(this Bitmap map)
        //{
        //    return GetImageFileExtension(map.RawFormat);
        //}
        //public static string GetImageFileExtension(this Image img)
        //{
        //    return GetImageFileExtension(new Bitmap(img.GetSourceStream()));
        //}
        public static string GetImagePath(this Image img)
        {
            if (img.Source != null) {
                var bs = img.GetBitmapSource();
                if (bs.UriSource != null)
                {
                    MessageBox.Show(bs.UriSource.LocalPath);
                    return bs.UriSource.LocalPath;
                }
                else return img.Tag as string;
            }
            return "";
        }
        public static bool IsUriDuplicated(this Image img, string path)
        {
            if (img.Source != null) {
                return img.Source.IsUriDuplicated(path);
            }
            return false;
        }
        public static bool IsUriDuplicated(this ImageSource img, string path)
        {

            if (img == null) return false;
            if (img.GetType() != typeof(BitmapImage)) return false;
            var bi = ((BitmapImage)img);
            if (bi.UriSource == null) return false;
            return bi.UriSource.Equals(new Uri(path)); ;
        }

        public static void Save(this Stream stream, string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            using (FileStream result = new FileStream(filePath, FileMode.Create))
            {
                stream.CopyTo(result);
            }
        }
        public static void Save(this Image img, string filePath)
        {
            using (var ms = img.GetSourceStream()) { ms.Save(filePath); }
        }
        public static byte[] ReadAllBytes(this Stream from)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = from.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public static Color GetPixelColor(this Image img, int x, int y)
        {
            BitmapSource source = img.Source as BitmapSource;
            Color c = Colors.White;
            if (source != null)
            {
                try
                {
                    CroppedBitmap cb = new CroppedBitmap(source, new Int32Rect(x, y, 1, 1));
                    var pixels = new byte[4];
                    cb.CopyPixels(pixels, 4, 0);
                    c = Color.FromArgb(pixels[3], pixels[0], pixels[1], pixels[2]);
                }
                catch (Exception) { }
            }
            return c;
        }
    }
}
