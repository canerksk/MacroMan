using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MacroMan.Services
{
    public class InputSimulator
    {
        #region Windows API for Alternative Method
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
        private const uint WM_SYSKEYDOWN = 0x0104;
        private const uint WM_SYSKEYUP = 0x0105;
        private const uint MAPVK_VK_TO_VSC = 0;

        private const uint WM_CHAR = 0x0102;
        private const uint WM_LBUTTONDOWN = 0x0201;
        private const uint WM_LBUTTONUP = 0x0202;
        private const uint WM_RBUTTONDOWN = 0x0204;
        private const uint WM_RBUTTONUP = 0x0205;

        #endregion


        #region Mouse API (global mouse için)
        [DllImport("user32.dll")]
        static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern short VkKeyScanEx(char ch, IntPtr dwhkl);


        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        #endregion

        private IntPtr _targetWindow = IntPtr.Zero;
        private uint _targetThreadId = 0;
        private readonly Control _ui;

        public InputSimulator(Control control)
        {
            _ui = control;
        }

        private bool HasValidTarget =>
            _targetWindow != IntPtr.Zero && NativeMethods.IsValidWindow(_targetWindow);

        public void SetTargetWindow(IntPtr windowHandle)
        {
            _targetWindow = windowHandle;
            _targetThreadId = (_targetWindow == IntPtr.Zero)
                ? 0u
                : NativeMethods.GetWindowThreadId(_targetWindow);

            string title = NativeMethods.GetWindowText(_targetWindow);
            string cls = NativeMethods.GetWindowClass(_targetWindow);

            Console.WriteLine($"[InputSimulator] Target window set: 0x{_targetWindow.ToInt64():X}, threadId={_targetThreadId}, title='{title}', class='{cls}'");
        }



        public void SendKeyPress(string keyName, bool ctrl = false, bool alt = false, bool shift = false)
        {
            if (string.IsNullOrWhiteSpace(keyName))
                return;

            Keys key = ParseKeyName(keyName);

            if (key == Keys.None)
            {
                Console.WriteLine($"[ERROR] Geçersiz tuş adı: {keyName}");
                return;
            }

            Console.WriteLine($"\n[SendKeyPress] START: keyName='{keyName}' -> Keys.{key}, Ctrl={ctrl}, Alt={alt}, Shift={shift}");
            SendKeyPress(key, ctrl, alt, shift);
        }


        public void SendKeyPress(Keys key, bool ctrl = false, bool alt = false, bool shift = false)
        {
            if (_ui != null && _ui.InvokeRequired)
            {
                Console.WriteLine("[SendKeyPress] UI thread gerekli, Invoke yapılıyor...");
                _ui.BeginInvoke(new Action(() => SendKeyPressInternal(key, ctrl, alt, shift)));
                return;
            }

            SendKeyPressInternal(key, ctrl, alt, shift);
        }

        public void SendTextByKeys(string text)
        {
            Console.WriteLine($"[SendTextByKeys] Text: '{text}'");

            if (string.IsNullOrEmpty(text))
                return;

            if (!HasValidTarget)
            {
                Console.WriteLine("[SendTextByKeys] Target yok, text atlanıyor.");
                return;
            }

            // Hedef thread'in keyboard layout'unu al
            IntPtr layout = NativeMethods.GetKeyboardLayout();

            foreach (char c in text)
            {
                short vkShift = VkKeyScanEx(c, layout);

                if (vkShift == -1)
                {
                    Console.WriteLine($"[SendTextByKeys] Char '{c}' için VK bulunamadı, atlanıyor.");
                    continue;
                }

                byte vk = (byte)(vkShift & 0xFF);
                byte shiftState = (byte)((vkShift >> 8) & 0xFF);

                bool shift = (shiftState & 1) != 0;
                bool ctrl = (shiftState & 2) != 0;
                bool alt = (shiftState & 4) != 0;

                Keys key = (Keys)vk;

                Console.WriteLine($"[SendTextByKeys] Char '{c}' -> VK={vk}(0x{vk:X}), Ctrl={ctrl}, Alt={alt}, Shift={shift}");

                // Burada senin zaten arkaplanda çalışan SendKeyPress yolunu kullanıyoruz
                SendKeyPress(key, ctrl, alt, shift);

                Thread.Sleep(10);
            }

            Console.WriteLine("[SendTextByKeys] Bitti.");
        }


        private void SendKeyPressInternal(Keys key, bool ctrl, bool alt, bool shift)
        {
            if (!HasValidTarget)
            {
                Console.WriteLine("[WARN] Geçersiz target window, system-wide SendKeys fallback...");
                SendKeyPressSystem(key, ctrl, alt, shift);
                return;
            }

            uint vk = (uint)key;
            Console.WriteLine($"[SendKeyPressInternal] Target=0x{_targetWindow.ToInt64():X}, VK={vk}(0x{vk:X}), Modifiers: Ctrl={ctrl}, Alt={alt}, Shift={shift}");

            if (ctrl || alt || shift)
            {
                Console.WriteLine("[METHOD] Modifier keys AKTIF - Method 1: ThreadInputAttach deneniyor...");
                
                bool method1Success = TrySendWithThreadAttach(key, vk, ctrl, alt, shift);
                
                if (!method1Success)
                {
                    Console.WriteLine("[METHOD] Method 1 BAŞARISIZ! Method 2: Direct PostMessage deneniyor...");
                    TrySendWithPostMessage(key, vk, ctrl, alt, shift);
                }
            }
            else
            {
                Console.WriteLine("[METHOD] Modifier YOK - Basit SendMessage kullanılıyor...");
                IntPtr layout = NativeMethods.GetKeyboardLayout();
                NativeMethods.SendKeyDown(_targetWindow, layout, vk);
                Thread.Sleep(30);
                NativeMethods.SendKeyUp(_targetWindow, layout, vk);
                Thread.Sleep(30);
                Console.WriteLine("[SUCCESS] ✓ Basit tuş gönderildi\n");
            }
        }

        private bool TrySendWithThreadAttach(Keys key, uint vk, bool ctrl, bool alt, bool shift)
        {
            uint thisThreadId = NativeMethods.GetThreadId();
            Console.WriteLine($"  [ThreadAttach] Bu thread: {thisThreadId}, Hedef thread: {_targetThreadId}");

            if (!NativeMethods.ThreadInputAttach(thisThreadId, _targetThreadId))
            {
                int error = Marshal.GetLastWin32Error();
                Console.WriteLine($"  [ERROR] ✗ ThreadInputAttach BAŞARISIZ! Win32Error: {error}");
                return false;
            }

            Console.WriteLine("  [ThreadAttach] ✓ ThreadInputAttach BAŞARILI!");

            try
            {
                byte[] state = NativeMethods.GetKeyboardState();
                Console.WriteLine($"  [KeyboardState] Mevcut state alındı (256 byte)");

                // Modifier keys set
                if (ctrl)
                {
                    state[0x11] |= 0x80; // VK_CONTROL
                    Console.WriteLine("    [Modifier] ✓ CTRL state set (0x11 |= 0x80)");
                }
                if (alt)
                {
                    state[0x12] |= 0x80; // VK_MENU
                    Console.WriteLine("    [Modifier] ✓ ALT state set (0x12 |= 0x80)");
                }
                if (shift)
                {
                    state[0x10] |= 0x80; // VK_SHIFT
                    Console.WriteLine("    [Modifier] ✓ SHIFT state set (0x10 |= 0x80)");
                }

                // Keyboard state
                NativeMethods.KeyboardState(state);
                Console.WriteLine("  [KeyboardState] ✓ SetKeyboardState çağrıldı");

                // send key
                IntPtr layout = NativeMethods.GetKeyboardLayout();
                Console.WriteLine($"  [SendKey] Keyboard Layout: 0x{layout.ToInt64():X}");

                Console.WriteLine($"  [SendKey] → WM_KEYDOWN gönderiliyor: VK={vk}(0x{vk:X}) = Keys.{key}");
                NativeMethods.SendKeyDown(_targetWindow, layout, vk);
                Thread.Sleep(50); // Biraz daha uzun bekle

                Console.WriteLine($"  [SendKey] → WM_KEYUP gönderiliyor: VK={vk}(0x{vk:X}) = Keys.{key}");
                NativeMethods.SendKeyUp(_targetWindow, layout, vk);
                Thread.Sleep(50);

                // Modifiers clear
                if (ctrl) state[0x11] &= 0x7F;
                if (alt) state[0x12] &= 0x7F;
                if (shift) state[0x10] &= 0x7F;

                NativeMethods.KeyboardState(state);
                Console.WriteLine("  [KeyboardState] ✓ Modifier keys temizlendi");

                Console.WriteLine("[SUCCESS] ✓ Method 1 BAŞARILI - ThreadInputAttach ile gönderildi\n");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [ERROR] Exception: {ex.Message}");
                return false;
            }
            finally
            {
                NativeMethods.ThreadInputDetach(thisThreadId, _targetThreadId);
                Console.WriteLine("  [ThreadAttach] ✓ ThreadInputDetach yapıldı");
            }
        }

        private void TrySendWithPostMessage(Keys key, uint vk, bool ctrl, bool alt, bool shift)
        {
            Console.WriteLine("  [PostMessage] Alternatif yöntem başlatılıyor...");

            try
            {
                uint scanCode = MapVirtualKey(vk, MAPVK_VK_TO_VSC);
                Console.WriteLine($"  [PostMessage] VK={vk}(0x{vk:X}), ScanCode={scanCode}(0x{scanCode:X})");

                // Modifier keys send (DOWN)
                if (ctrl)
                {
                    Console.WriteLine("    [PostMessage] → CTRL DOWN (VK=0x11)");
                    PostKeyMessage(_targetWindow, WM_KEYDOWN, 0x11, scanCode);
                    Thread.Sleep(20);
                }
                if (alt)
                {
                    Console.WriteLine("    [PostMessage] → ALT DOWN (VK=0x12)");
                    PostKeyMessage(_targetWindow, WM_SYSKEYDOWN, 0x12, scanCode);
                    Thread.Sleep(20);
                }
                if (shift)
                {
                    Console.WriteLine("    [PostMessage] → SHIFT DOWN (VK=0x10)");
                    PostKeyMessage(_targetWindow, WM_KEYDOWN, 0x10, scanCode);
                    Thread.Sleep(20);
                }

                // main key send
                Console.WriteLine($"  [PostMessage] → MAIN KEY DOWN: VK={vk}(0x{vk:X}) = Keys.{key}");
                PostKeyMessage(_targetWindow, alt ? WM_SYSKEYDOWN : WM_KEYDOWN, vk, scanCode);
                Thread.Sleep(50);

                Console.WriteLine($"  [PostMessage] → MAIN KEY UP: VK={vk}(0x{vk:X}) = Keys.{key}");
                PostKeyMessage(_targetWindow, alt ? WM_SYSKEYUP : WM_KEYUP, vk, scanCode);
                Thread.Sleep(50);

                // Modifier keys send (UP)
                if (shift)
                {
                    Console.WriteLine("    [PostMessage] → SHIFT UP (VK=0x10)");
                    PostKeyMessage(_targetWindow, WM_KEYUP, 0x10, scanCode);
                    Thread.Sleep(20);
                }
                if (alt)
                {
                    Console.WriteLine("    [PostMessage] → ALT UP (VK=0x12)");
                    PostKeyMessage(_targetWindow, WM_SYSKEYUP, 0x12, scanCode);
                    Thread.Sleep(20);
                }
                if (ctrl)
                {
                    Console.WriteLine("    [PostMessage] → CTRL UP (VK=0x11)");
                    PostKeyMessage(_targetWindow, WM_KEYUP, 0x11, scanCode);
                    Thread.Sleep(20);
                }

                Console.WriteLine("[SUCCESS] ✓ Method 2 TAMAMLANDI - PostMessage ile gönderildi\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [ERROR] PostMessage Exception: {ex.Message}");
                // Son çare: SendKeys fallback
                Console.WriteLine("  [FALLBACK] SendKeys kullanılıyor...");
                SendKeyPressSystem(key, ctrl, alt, shift);
            }
        }

        private void PostKeyMessage(IntPtr hWnd, uint msg, uint vk, uint scanCode)
        {
            // lParam = repeat count (1) + scan code (bits 16-23) + extended key flag + context code + previous state + transition state
            IntPtr lParam;
            
            if (msg == WM_KEYUP || msg == WM_SYSKEYUP)
            {
                // Key up: bit 30 (previous state=1) + bit 31 (transition state=1)
                lParam = (IntPtr)((scanCode << 16) | 0xC0000001);
            }
            else
            {
                // Key down
                lParam = (IntPtr)((scanCode << 16) | 0x00000001);
            }

            PostMessage(hWnd, msg, (IntPtr)vk, lParam);
        }

        private void SendKeyPressSystem(Keys key, bool ctrl, bool alt, bool shift)
        {
            string keyString = GetSendKeysString(key, ctrl, alt, shift);
            if (!string.IsNullOrEmpty(keyString))
            {
                Console.WriteLine($"[SendKeys] System-wide fallback: {keyString}");
                SendKeys.SendWait(keyString);
            }
        }

        private string GetSendKeysString(Keys key, bool ctrl, bool alt, bool shift)
        {
            string modifiers = "";
            if (ctrl) modifiers += "^";
            if (alt) modifiers += "%";
            if (shift) modifiers += "+";

            string keyStr = key switch
            {
                Keys.F1 => "{F1}",
                Keys.F2 => "{F2}",
                Keys.F3 => "{F3}",
                Keys.F4 => "{F4}",
                Keys.F5 => "{F5}",
                Keys.F6 => "{F6}",
                Keys.F7 => "{F7}",
                Keys.F8 => "{F8}",
                Keys.F9 => "{F9}",
                Keys.F10 => "{F10}",
                Keys.F11 => "{F11}",
                Keys.F12 => "{F12}",
                Keys.Left => "{LEFT}",
                Keys.Right => "{RIGHT}",
                Keys.Up => "{UP}",
                Keys.Down => "{DOWN}",
                Keys.Enter => "{ENTER}",
                Keys.Space => " ",
                Keys.Tab => "{TAB}",
                Keys.Escape => "{ESC}",
                _ => key.ToString()
            };

            return modifiers + keyStr;
        }

        // -----------------------------
        //  TEXT SEND
        // -----------------------------
        public void SendTextToTarget(string text)
        {
            Console.WriteLine($"[SendTextToTarget] Text: '{text}'");

            if (string.IsNullOrEmpty(text))
                return;

            if (_targetWindow == IntPtr.Zero)
            {
                Console.WriteLine("[SendTextToTarget] Target window yok (0), text atlanıyor.");
                return;
            }

            // İstersen burada IsValidWindow kontrolü de yapabilirsin ama
            // makro sırasında handle zaten biliniyor varsayıyoruz
            foreach (char c in text)
            {
                // Karakteri direk hedef pencereye gönder
                PostMessage(_targetWindow, WM_CHAR, (IntPtr)c, IntPtr.Zero);
                Thread.Sleep(10);
            }

            Console.WriteLine("[SendTextToTarget] WM_CHAR mesajları gönderildi.");
        }


        public void SendText(string text)
        {
            Console.WriteLine($"[SendText] Text: '{text}'");

            if (string.IsNullOrEmpty(text))
                return;

            if (!HasValidTarget)
            {
                Console.WriteLine("[SendText] HasValidTarget = FALSE, hiçbir şey yapmıyorum.");
                return; // SendKeys YOK ARTIK
            }

            Console.WriteLine($"[SendText] WM_CHAR ile target=0x{_targetWindow.ToInt64():X} penceresine yazılıyor...");

            foreach (char c in text)
            {
                PostMessage(_targetWindow, WM_CHAR, (IntPtr)c, IntPtr.Zero);
                Thread.Sleep(10);
            }

            Console.WriteLine("[SendText] WM_CHAR mesajları bitti.");
        }



        public void SendEnterToTarget()
        {
            const uint VK_RETURN = 0x0D;

            // Hedef pencere yoksa: aktif pencereye Enter gönder
            if (!HasValidTarget)
            {
                SendKeys.SendWait("{ENTER}");
                return;
            }

            uint scanCode = MapVirtualKey(VK_RETURN, MAPVK_VK_TO_VSC);

            // lParam formatı:
            // bit 0-15: repeat count (1)
            // bit 16-23: scan code
            // bit 30: previous key state
            // bit 31: transition state

            IntPtr lParamDown = (IntPtr)((scanCode << 16) | 0x00000001);      // first press
            IntPtr lParamUp = (IntPtr)((scanCode << 16) | 0xC0000001);      // key up

            Console.WriteLine("[SendEnterToTarget] WM_KEYDOWN/UP VK_RETURN gönderiliyor");

            PostMessage(_targetWindow, WM_KEYDOWN, (IntPtr)VK_RETURN, lParamDown);
            Thread.Sleep(30);
            PostMessage(_targetWindow, WM_KEYUP, (IntPtr)VK_RETURN, lParamUp);
        }


        // -----------------------------
        //  MOUSE (global)
        // -----------------------------

        public void SendMouseClick(int x, int y, Models.MouseButton button, Models.ClickType clickType)
        {
            Console.WriteLine($"[MouseClick] X={x}, Y={y}, Button={button}, Type={clickType}");

            if (!HasValidTarget)
            {
                Console.WriteLine("[MouseClick] Geçerli target yok, click atlanıyor.");
                return;
            }

            // Ekran koordinatını client koordinatına çevir
            POINT pt = new POINT { X = x, Y = y };
            if (!ScreenToClient(_targetWindow, ref pt))
            {
                Console.WriteLine("[MouseClick] ScreenToClient başarısız, click atlanıyor.");
                return;
            }

            int clientX = pt.X;
            int clientY = pt.Y;

            Console.WriteLine($"[MouseClick] client = ({clientX},{clientY})");

            const uint WM_MOUSEMOVE = 0x0200;
            const int MK_LBUTTON = 0x0001;
            const int MK_RBUTTON = 0x0002;

            uint downMsg = button == Models.MouseButton.Sol ? WM_LBUTTONDOWN : WM_RBUTTONDOWN;
            uint upMsg = button == Models.MouseButton.Sol ? WM_LBUTTONUP : WM_RBUTTONUP;
            int mouseKeyFlag = button == Models.MouseButton.Sol ? MK_LBUTTON : MK_RBUTTON;

            int clicks = clickType == Models.ClickType.Tek ? 1 : 2;

            IntPtr lParam = (IntPtr)((clientY << 16) | (clientX & 0xFFFF));
            IntPtr wParamDown = (IntPtr)mouseKeyFlag;

            for (int i = 0; i < clicks; i++)
            {
                // Mouse'u o noktaya götürülmüş gibi göster
                SendMessage(_targetWindow, WM_MOUSEMOVE, IntPtr.Zero, lParam);
                Thread.Sleep(10);

                // Down
                SendMessage(_targetWindow, downMsg, wParamDown, lParam);
                Thread.Sleep(10);

                // Up
                SendMessage(_targetWindow, upMsg, IntPtr.Zero, lParam);
                Thread.Sleep(50);

                if (i < clicks - 1)
                    Thread.Sleep(50);
            }

            Console.WriteLine("[MouseClick] SendMessage ile click gönderildi.");
        }





        #region Key Name Parsing
        private Keys ParseKeyName(string keyName)
        {
            return keyName.ToLower() switch
            {
                "f1" => Keys.F1,
                "f2" => Keys.F2,
                "f3" => Keys.F3,
                "f4" => Keys.F4,
                "f5" => Keys.F5,
                "f6" => Keys.F6,
                "f7" => Keys.F7,
                "f8" => Keys.F8,
                "f9" => Keys.F9,
                "f10" => Keys.F10,
                "f11" => Keys.F11,
                "f12" => Keys.F12,
                "sol" or "left" => Keys.Left,
                "sag" or "right" => Keys.Right,
                "yukari" or "up" => Keys.Up,
                "asagi" or "down" => Keys.Down,
                "enter" => Keys.Enter,
                "space" or "bosluk" => Keys.Space,
                "tab" => Keys.Tab,
                "esc" or "escape" => Keys.Escape,
                _ => ParseSingleKey(keyName)
            };
        }

        private Keys ParseSingleKey(string keyName)
        {
            if (keyName.Length == 1)
            {
                char c = char.ToUpper(keyName[0]);
                if (c >= 'A' && c <= 'Z')
                    return (Keys)c;
                if (c >= '0' && c <= '9')
                    return (Keys)c;
            }
            return Keys.None;
        }
        #endregion
    }
}
