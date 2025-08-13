using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Dms.HMI
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool IsNewProcess;
            Mutex duplicate = new Mutex(true, "WSSD_New", out IsNewProcess);

            if (IsNewProcess)
            {
                //////////////////////////////////////////////////////////////////////
                // Process 우선순위 -> High
                Process currentProcess = Process.GetCurrentProcess();
                currentProcess.PriorityClass = ProcessPriorityClass.High;
                //////////////////////////////////////////////////////////////////////

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                duplicate.ReleaseMutex();
            }
            else
            {
                return;
            }
        }
    }
}