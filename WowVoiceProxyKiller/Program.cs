using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace WowVoiceProxyKiller
{
    public class Program
    {
        public static readonly string[] ProcessNames = { "WowVoiceProxy", "WowVoiceProxyT", "World of Warcraft Voice Proxy" };
        public static bool IsRunning = true;
        public static int SleepInterval = 10000;
        public static int TotalKilled;

        public static void Main(string[] args)
        {
            Console.WriteLine("Process started, press ctrl+c to exit");

            Console.CancelKeyPress += Console_CancelKeyPress;

            while (IsRunning)
            {
                GetProcesses().ForEach(process =>
                {
                    if (process.HasExited) return;

                    if (process.MainWindowHandle == IntPtr.Zero)
                    {
                        process.Kill();
                    }
                    else
                    {
                        process.CloseMainWindow();
                        process.WaitForExit();
                    }
                    
                    process.Dispose();

                    TotalKilled++;
                    Console.WriteLine($"Killed wow voice proxy process, Total times killed: {TotalKilled} ");
                });

                Thread.Sleep(SleepInterval);
            }
        }

        private static List<Process> GetProcesses()
        {
            var foundProcesses = new List<Process>();
            
            foreach (var processName in ProcessNames)
            {
                foundProcesses.AddRange(Process.GetProcessesByName(processName));
            }

            return foundProcesses;
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Exiting WowVoiceProxyKiller..");
            Console.WriteLine($"Total processes killed: {TotalKilled}");
            IsRunning = false;
            Environment.Exit(0);
        }
    }
}
