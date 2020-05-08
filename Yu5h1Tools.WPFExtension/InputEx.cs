using System.Windows.Input;


namespace Yu5h1Tools.WPFExtension
{
    public static class KeyboardUtility
    {
        public static bool IsPressed(this Key[] keys) {
            if (keys.Length == 0) return false;
            foreach (var key in keys)
            {
                if (!Keyboard.IsKeyDown(key)) return false;
            }
            return true;
        }
        public static bool IsPressed(this Key key) {
            return Keyboard.IsKeyDown(key);
        }
    }
}
