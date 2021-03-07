using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ForkerionAPI
{
    internal class Pipe
    {
        public Pipe()
        {
        }

        public static void MainPipeClient(string pipe, string input)
        {
            var Err = new Forkerion.ErrorMessage();

            if (!Pipe.NamedPipeExist(pipe))
            {
                Forkerion.Properties.Settings.Default.ErrorMessage = "Dll not found, please disable you antivirus!";
                Forkerion.Properties.Settings.Default.Save();

                Err.Show();
            }
            else
            {
                try
                {
                    using (NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", pipe, PipeDirection.Out))
                    {
                        namedPipeClientStream.Connect();
                        using (StreamWriter streamWriter = new StreamWriter(namedPipeClientStream))
                        {
                            streamWriter.Write(input);
                            streamWriter.Dispose();
                        }
                        namedPipeClientStream.Dispose();
                    }
                }
                catch (IOException oException)
                {
                    Forkerion.Properties.Settings.Default.ErrorMessage = "Incorrect Pipe!";
                    Forkerion.Properties.Settings.Default.Save();

                    Err.Show();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message.ToString());
                }
            }
        }

        public static bool NamedPipeExist(string pipeName)
        {
            bool flag;
            bool flag1;
            try
            {
                if (!Pipe.WaitNamedPipe(Path.GetFullPath(String.Format("\\\\.\\pipe\\{0}", pipeName)), 0))
                {
                    int lastWin32Error = Marshal.GetLastWin32Error();
                    if (lastWin32Error == 0)
                    {
                        flag1 = false;
                        return flag1;
                    }
                    else if (lastWin32Error == 2)
                    {
                        flag1 = false;
                        return flag1;
                    }
                }
                flag = true;
            }
            catch (Exception exception)
            {
                flag = false;
            }
            flag1 = flag;
            return flag1;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
        private static extern bool WaitNamedPipe(string name, int timeout);

        public class BasicInject
        {
            public BasicInject()
            {
            }

            [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            internal static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, UIntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

            [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            internal static extern bool FreeLibrary(IntPtr hModule);

            [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
            internal static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);

            public bool InjectDLL()
            {
                UIntPtr uIntPtr;
                IntPtr intPtr;
                bool flag;
                if (Process.GetProcessesByName("RobloxPlayerBeta").Length != 0)
                {
                    Process processesByName = Process.GetProcessesByName("RobloxPlayerBeta")[0];
                    byte[] bytes = (new ASCIIEncoding()).GetBytes(String.Concat(AppDomain.CurrentDomain.BaseDirectory, Client.name));
                    IntPtr intPtr1 = Pipe.BasicInject.LoadLibraryA("kernel32.dll");
                    UIntPtr procAddress = Pipe.BasicInject.GetProcAddress(intPtr1, "LoadLibraryA");
                    Pipe.BasicInject.FreeLibrary(intPtr1);
                    if (procAddress != UIntPtr.Zero)
                    {
                        IntPtr intPtr2 = Pipe.BasicInject.OpenProcess(Pipe.BasicInject.ProcessAccess.AllAccess, false, processesByName.Id);
                        if (intPtr2 != IntPtr.Zero)
                        {
                            IntPtr intPtr3 = Pipe.BasicInject.VirtualAllocEx(intPtr2, (IntPtr)0, (uint)bytes.Length, 12288, 4);
                            flag = (intPtr3 == IntPtr.Zero || !Pipe.BasicInject.WriteProcessMemory(intPtr2, intPtr3, bytes, (uint)bytes.Length, out uIntPtr) ? false : !(Pipe.BasicInject.CreateRemoteThread(intPtr2, (IntPtr)0, 0, procAddress, intPtr3, 0, out intPtr) == IntPtr.Zero));
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
                return flag;
            }

            [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = false, SetLastError = true)]
            internal static extern IntPtr LoadLibraryA(string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            internal static extern IntPtr OpenProcess(Pipe.BasicInject.ProcessAccess dwDesiredAccess, bool bInheritHandle, int dwProcessId);

            [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

            [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = true, SetLastError = true)]
            internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

            [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

            [Flags]
            public enum ProcessAccess
            {
                Terminate = 1,
                CreateThread = 2,
                VMOperation = 8,
                VMRead = 16,
                VMWrite = 32,
                DuplicateHandle = 64,
                SetInformation = 512,
                QueryInformation = 1024,
                Synchronize = 1048576,
                AllAccess = 1050235
            }
        }
    }
}