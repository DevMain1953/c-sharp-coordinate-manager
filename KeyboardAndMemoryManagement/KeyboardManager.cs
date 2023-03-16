using WinAPI;

namespace KeyboardManagement
{
    public class KeyboardManager : WindowsAPI
    {
        public static bool IsKeyPushedDown(System.Int32 codeOfVirtualKey)
        {
            return (GetAsyncKeyState(codeOfVirtualKey) & 0x8000) != 0;
        }
    }
}
