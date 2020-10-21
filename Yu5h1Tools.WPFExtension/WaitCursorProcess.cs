using System;
using System.Windows.Input;
using System.Media;
namespace Yu5h1Tools.WPFExtension
{
    public sealed class WaitCursorProcess : IDisposable
    {
        bool PlayCompletedSound = true;
        public bool succeed = false;
        public WaitCursorProcess(bool playCompletedSound = true)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            PlayCompletedSound = playCompletedSound;
            System.Threading.Thread.Sleep(50);
        }
        public void Dispose()
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            if (PlayCompletedSound) {
                if (succeed) SystemSounds.Asterisk.Play();
            } 
        }
    }
}
