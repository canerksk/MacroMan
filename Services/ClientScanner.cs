using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MacroMan.Services
{
    public class UOClient
    {
        public string Name { get; set; }
        public IntPtr Handle { get; set; } 
        public int ProcessId { get; set; }
        public uint ThreadId { get; set; }
        public ClientType Type { get; set; }

        public override string ToString()
        {
            string typeStr = Type switch
            {
                ClientType.ClassicDirect => "[Classic]",
                ClientType.EnhancedDirect => "[Enhanced]",
                ClientType.OrionDirect => "[Orion]",
                ClientType.Assistant => "",
                _ => ""
            };
            //return $"{typeStr} {Name} (Handle: {Handle})";
            return $"{typeStr} {Name}";
        }
    }

    public enum ClientType
    {
        ClassicDirect,
        EnhancedDirect,
        OrionDirect,
        Assistant
    }

    public class ClientScanner
    {
        #region Windows API

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindow(IntPtr hWnd);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        private const uint WM_CLOSE = 0x0010;

        // Assistant window class name
        private const string ASSISTUO_CLASS = "UOASSIST-TP-MSG-WND";

        // Process names
        private const string UO_CLASSIC_PROCESS = "client";
        private const string UO_ENHANCED_PROCESS = "UOSA";
        private const string UO_ORION_PROCESS = "OrionUO";

        #endregion

        private List<UOClient> _clients;

        public List<UOClient> ScanForClients()
        {
            _clients = new List<UOClient>();

            // Method 1: Find by process name (direct client detection)
            ScanByProcessName();

            // Method 2: Find AssistUO windows (assistant detection)
            ScanByAssistant();

            // Method 3: EnumWindows fallback (class/title based)
            EnumWindows(EnumerateWindow, IntPtr.Zero);

            return _clients;
        }

        private void ScanByProcessName()
        {
            // Classic Client
            try
            {
                Process[] classicClients = Process.GetProcessesByName(UO_CLASSIC_PROCESS);
                foreach (var process in classicClients)
                {
                    if (process.MainWindowHandle != IntPtr.Zero && IsWindowVisible(process.MainWindowHandle))
                    {
                        string title = GetWindowTitle(process.MainWindowHandle);
                        if (!string.IsNullOrEmpty(title))
                        {
                            AddClient(new UOClient
                            {
                                Name = title,
                                Handle = process.MainWindowHandle,
                                ProcessId = process.Id,
                                ThreadId = GetThreadId(process.MainWindowHandle),
                                Type = ClientType.ClassicDirect
                            });
                        }
                    }
                }
            }
            catch { }

            // Enhanced Client
            try
            {
                Process[] enhancedClients = Process.GetProcessesByName(UO_ENHANCED_PROCESS);
                foreach (var process in enhancedClients)
                {
                    if (process.MainWindowHandle != IntPtr.Zero && IsWindowVisible(process.MainWindowHandle))
                    {
                        string title = GetWindowTitle(process.MainWindowHandle);
                        if (!string.IsNullOrEmpty(title))
                        {
                            AddClient(new UOClient
                            {
                                Name = title,
                                Handle = process.MainWindowHandle,
                                ProcessId = process.Id,
                                ThreadId = GetThreadId(process.MainWindowHandle),
                                Type = ClientType.EnhancedDirect
                            });
                        }
                    }
                }
            }
            catch { }

            // Orion Client
            try
            {
                Process[] orionClients = Process.GetProcessesByName(UO_ORION_PROCESS);
                foreach (var process in orionClients)
                {
                    if (process.MainWindowHandle != IntPtr.Zero && IsWindowVisible(process.MainWindowHandle))
                    {
                        string title = GetWindowTitle(process.MainWindowHandle);
                        if (!string.IsNullOrEmpty(title))
                        {
                            AddClient(new UOClient
                            {
                                Name = title,
                                Handle = process.MainWindowHandle,
                                ProcessId = process.Id,
                                ThreadId = GetThreadId(process.MainWindowHandle),
                                Type = ClientType.OrionDirect
                            });
                        }
                    }
                }
            }
            catch { }
        }

        private void ScanByAssistant()
        {
            List<IntPtr> assistWindows = FindWindowsWithText(ASSISTUO_CLASS);

            foreach (IntPtr hwnd in assistWindows)
            {
                try
                {
                    GetWindowThreadProcessId(hwnd, out int pid);
                    Process process = Process.GetProcessById(pid);

                    if (process == null)
                        continue;

                    IntPtr gameHandle = process.MainWindowHandle;
                    if (gameHandle == IntPtr.Zero || !IsWindowVisible(gameHandle))
                        continue;

                    string title = GetWindowTitle(gameHandle);
                    if (string.IsNullOrEmpty(title))
                        title = process.MainWindowTitle;

                    if (string.IsNullOrEmpty(title))
                        continue;

                    AddClient(new UOClient
                    {
                        Name = title,
                        Handle = gameHandle,                       // game window
                        ProcessId = pid,
                        ThreadId = GetThreadId(gameHandle),
                        Type = ClientType.Assistant
                    });
                }
                catch { }
            }
        }

        private bool EnumerateWindow(IntPtr windowHandle, IntPtr lParameter)
        {
            if (!IsWindowVisible(windowHandle))
                return true;

            try
            {
                string windowClass = GetWindowClass(windowHandle);
                string windowTitle = GetWindowTitle(windowHandle);

                if (windowClass.Contains(ASSISTUO_CLASS))
                {
                    GetWindowThreadProcessId(windowHandle, out int pid);

                    try
                    {
                        Process process = Process.GetProcessById(pid);
                        if (process == null)
                            return true;

                        IntPtr gameHandle = process.MainWindowHandle;
                        if (gameHandle == IntPtr.Zero || !IsWindowVisible(gameHandle))
                            return true;

                        string title = GetWindowTitle(gameHandle);
                        if (string.IsNullOrEmpty(title))
                            title = process.MainWindowTitle;

                        if (string.IsNullOrEmpty(title))
                            return true;

                        if (!_clients.Any(c => c.Handle == gameHandle))
                        {
                            AddClient(new UOClient
                            {
                                Name = title,
                                Handle = gameHandle,                      // game window
                                ProcessId = pid,
                                ThreadId = GetThreadId(gameHandle),
                                Type = ClientType.Assistant
                            });
                        }
                    }
                    catch { }

                    return true;
                }

                // UO class name kontrolü
                if (windowClass.Contains("Ultima Online") || windowClass.Contains("UO"))
                {
                    if (!string.IsNullOrEmpty(windowTitle) && windowTitle.Contains("Ultima Online"))
                    {
                        GetWindowThreadProcessId(windowHandle, out int pid);

                        if (!_clients.Any(c => c.Handle == windowHandle))
                        {
                            ClientType type = ClientType.ClassicDirect;

                            try
                            {
                                Process p = Process.GetProcessById(pid);
                                string pname = p.ProcessName.ToLowerInvariant();
                                if (pname.Contains("uosa"))
                                    type = ClientType.EnhancedDirect;
                                else if (pname.Contains("orion"))
                                    type = ClientType.OrionDirect;
                            }
                            catch { }

                            AddClient(new UOClient
                            {
                                Name = windowTitle,
                                Handle = windowHandle,
                                ProcessId = pid,
                                ThreadId = GetThreadId(windowHandle),
                                Type = type
                            });
                        }
                    }
                }
            }
            catch { }

            return true;
        }

        private List<IntPtr> FindWindowsWithText(string text)
        {
            List<IntPtr> windows = new List<IntPtr>();

            EnumWindows((hwnd, lParam) =>
            {
                string className = GetWindowClass(hwnd);
                if (className.Contains(text))
                {
                    windows.Add(hwnd);
                }
                return true;
            }, IntPtr.Zero);

            return windows;
        }

        private void AddClient(UOClient client)
        {
            // Duplicate kontrolü: aynı handle varsa ekleme
            if (!_clients.Any(c => c.Handle == client.Handle))
            {
                _clients.Add(client);
            }
        }

        private string GetWindowTitle(IntPtr hWnd)
        {
            try
            {
                int length = GetWindowTextLength(hWnd);
                if (length > 0)
                {
                    StringBuilder sb = new StringBuilder(length + 1);
                    GetWindowText(hWnd, sb, sb.Capacity);
                    return sb.ToString();
                }
            }
            catch { }
            return string.Empty;
        }

        private string GetWindowClass(IntPtr hWnd)
        {
            try
            {
                StringBuilder sb = new StringBuilder(256);
                GetClassName(hWnd, sb, sb.Capacity);
                return sb.ToString();
            }
            catch { }
            return string.Empty;
        }

        private uint GetThreadId(IntPtr hWnd)
        {
            uint threadId = GetWindowThreadProcessId(hWnd, out int _);
            return threadId;
        }

        public static void CloseClient(IntPtr handle)
        {
            try
            {
                SendMessage(handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
            catch { }
        }

        public static bool IsValidWindow(IntPtr handle)
        {
            return handle != IntPtr.Zero && IsWindow(handle);
        }
    }
}
