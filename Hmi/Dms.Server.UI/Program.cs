using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Dms.Server;
using Dms.Device;
using System.Diagnostics;

namespace TestServer
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //////////////////////////////////////////////////////////////////////
            // Process 우선순위 -> RealTime
            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.PriorityClass = ProcessPriorityClass.High;
            //////////////////////////////////////////////////////////////////////

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FormServer form = new FormServer();
            form.Initialize(true);
            Application.Run(form);
        }
    }
}