using System.Windows.Input;


namespace Yu5h1Tools.WPFExtension
{
    public static class InputEx
    {
        public static bool KeysDown(params Key[] keys) {
            foreach (var key in keys)
            {
                if (!Keyboard.IsKeyDown(key)) return false;
            }
            return true;
        }
    }
}
