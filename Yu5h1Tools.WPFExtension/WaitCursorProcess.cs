using System;
using System.Windows.Input;

namespace Yu5h1Tools.WPFExtension
{
    public sealed class WaitCursorProcess : IDisposable
    {
        public bool PlayCompletedSound = false;
        public WaitCursorProcess(bool playCompletedSound = false)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            PlayCompletedSound = playCompletedSound;
        }
        public void Dispose()
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            if (PlayCompletedSound) System.Media.SystemSounds.Asterisk.Play();
        }
    }
}
